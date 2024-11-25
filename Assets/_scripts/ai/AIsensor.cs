using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

/// <summary>
/// cette classe contient plein de références et de méthodes qui permettent 
/// aux différents states du bot d'acceder au contenu du jeu.
/// </summary>
public class AiSensor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] HealthComponent _playerHPcomponent;
    [SerializeField] HealthComponent _AiHPcomponent;
    [SerializeField] BombBag _AiBombBag;
    [SerializeField] BombBag _playerBombBag;
    [SerializeField] Move _playerMovement;
    [SerializeField] Move _agentMovement;


    //getters
    public int PlayerHP => _playerHPcomponent.HP;
    public int AgentHP => _playerHPcomponent.HP;
    public int AgentBombCount => _AiBombBag.BombStack.Count;

    public Vector2Int PlayerPosition => _playerMovement.CurrentNode.pose;

    //events
    public event Action OnBombPickedUpByAgent;
    public event Action OnBombPickedUpByPlayer;

    public event Action OnPlayerMoved;
    public event Action OnAgentMoved;

    public event Action OnBombPlacedByAgent;
    public event Action OnBombPlacedByPlayer;
    /// <summary>
    /// trouve la bombe ramassable la plus proche
    /// </summary>
    /// <returns></returns>
    public TileAstarNode FindNearestBombNode()
    {
        float minDistanceSquared = float.PositiveInfinity;
        Transform closest = null;
        foreach (BombItem bomb in BombItem.freeBombs)
        {
            float dsq = (bomb.transform.position - transform.position).sqrMagnitude;
            if (dsq < minDistanceSquared)
            {
                minDistanceSquared = dsq;
                closest = bomb.transform;
            } 
        }
        return closest == null ? null : Graph.Instance.Nodes[closest.position.RoundToV2Int()];
    }
    
    /// <summary>
    /// renvoie le noeud le plus proche du joueur (par rapport au bot) si il y'en a un de libre. renvoie null sinon
    /// </summary>
    /// <returns></returns>
    public TileAstarNode FindNearestNodeAroundPlayer()
    {
        float minDistanceSquared = float.PositiveInfinity;
        Vector2Int closest = Vector2Int.zero;

        foreach (Vector2Int offset in VectorExtensions.AllFourDirections)
        {
            Vector2Int pose = _playerMovement.CurrentNode.pose + offset;
            if (Graph.Instance.Nodes.ContainsKey(pose) && Graph.Instance.Nodes[pose].isActive())
            {
                float DistanceSquared = (pose - (Vector2)transform.position).sqrMagnitude;
                if (DistanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = DistanceSquared;
                    closest = pose;
                }
            }
        }

        if(closest != Vector2Int.zero) return Graph.Instance.Nodes[closest];
        return null;
    }

    /// <summary>
    /// retourne la liste des bombes pouvant potentiellement faire des degats au bot si il est sur le point donné
    /// </summary>
    /// <returns></returns>
    public List<Bomb> FindTickingBombsAroundPoint(Vector2 point)
    {
        List<Bomb> output = new();

        foreach (Vector2Int offset in VectorExtensions.AllFourDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(point.Round() + (Vector2)offset * 0.51f, offset, 3, ~LayerMask.GetMask(Graph.NodeLayer));
            if (hit && hit.collider.gameObject.TryGetComponent<Bomb>(out Bomb bomb)) output.Add(bomb);
        }

        return output;
    }

    public void FindTickingBombsAroundPoint_NoAlloc(Vector2 point,ref List<Bomb> output)
    {
        output.Clear();

        foreach (Vector2Int offset in VectorExtensions.AllFourDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(point , offset, 3, LayerMask.GetMask("Solid"));

            if (hit && hit.collider.gameObject.TryGetComponent<Bomb>(out Bomb bomb)) output.Add(bomb);
        }
    }

    public bool IsTileSafe(Vector2 point)
    {
        foreach (Vector2Int offset in VectorExtensions.AllFourDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(point.Round(), offset, 3, LayerMask.GetMask("Solid"));
            if (hit && hit.collider.gameObject.TryGetComponent<Bomb>(out Bomb bomb)) return false;
        }
        return true;
    }

    /// <summary>
    /// trouve les cases libres autour de la position donnée
    /// </summary>
    /// <param name="point"></param>
    public List<Vector2Int> FindFreeNodesAroundPoint(Vector2 point)
    {
        List<Vector2Int> output = new();
        foreach (Vector2Int offset in VectorExtensions.AllFourDirections)
        {
            Vector2Int pose = point.RoundToV2Int() + offset;
            if(Graph.Instance.Nodes.ContainsKey(pose)) output.Add(point.RoundToV2Int() + offset);
        }
        return output;
    }

    //set up events
    private void Awake()
    {
        _playerBombBag.OnBombPickedUp += () => OnBombPickedUpByPlayer?.Invoke();
        _AiBombBag.OnBombPickedUp += () => OnBombPickedUpByAgent?.Invoke();
        _playerMovement.OnMove += () => OnPlayerMoved?.Invoke();
        _agentMovement.OnMove += () => OnAgentMoved?.Invoke();
        _AiBombBag.OnBombPlaced += () => OnBombPlacedByAgent?.Invoke();
        _playerBombBag.OnBombPlaced += () => OnBombPlacedByPlayer?.Invoke();
    }
}

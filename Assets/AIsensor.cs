using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// cette classe contient plein de références et de méthodes qui permettent 
/// aux différents states de la state machine de l'IA d'acceder au contenu du jeu.
/// </summary>
public class AIsensor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] HealthComponent _playerHPcomponent;
    [SerializeField] HealthComponent _AiHPcomponent;
    [SerializeField] BombBag _AibombBag;
    [SerializeField] Move _playerMovement;


    //getters
    public int PlayerHP => _playerHPcomponent.HP;
    public int AgentHP => _playerHPcomponent.HP;
    public int AgentBombCount => _AibombBag.BombStack.Count;

    //events
    public event Action OnBombPickedUpByAgent;
    public event Action OnPlayerMoved;

    /// <summary>
    /// trouve la bombe ramassable la plus proche
    /// </summary>
    /// <returns></returns>
    public Node FindNearestBombNode()
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
        return closest == null ? null : Graph.Instance.Nodes[closest.position.ToVector2Int()];
    }
    
    /// <summary>
    /// renvoie le noeud le plus proche du joueur si il y'en a un de libre. renvoie null sinon
    /// </summary>
    /// <returns></returns>
    public Node FindNearestNodeAroundPlayer()
    {
        float minDistanceSquared = float.PositiveInfinity;
        Vector2Int closest = Vector2Int.zero;

        foreach (Vector2Int offset in VectorExtensions.AllFourDirections)
        {
            Vector2Int pose = _playerMovement.CurrentNode.pose + offset;
            if (Graph.Instance.Nodes.ContainsKey(pose) && Graph.Instance.Nodes[pose].isActiveAndEnabled)
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

    //set up events
    private void Awake()
    {
        _AibombBag.OnBombPickedUp += () => OnBombPickedUpByAgent?.Invoke();
        _playerMovement.OnMove += () => OnPlayerMoved?.Invoke();
    }
}

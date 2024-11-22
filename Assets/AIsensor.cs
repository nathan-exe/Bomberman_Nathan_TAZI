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
    /// renvoie le noeud sur lequel se trouve actuellement le joueur
    /// </summary>
    /// <returns></returns>
    public Node GetPlayerNode()
    {
        return _playerMovement.CurrentNode;
    }

    private void Awake()
    {
        _AibombBag.OnBombPickedUp += () => OnBombPickedUpByAgent?.Invoke();
    }
}

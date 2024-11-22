using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_ChasingPlayer : StateBase
{
    Node _currentTarget;
    public State_ChasingPlayer(StateMachine sm) : base(sm)
    {
    }

    public override bool canTransitionToState(StateConditions conditions)
    {
        throw new System.NotImplementedException();
    }

    public override float computeScore(StateOutcome outcome, StateWeights weights)
    {
        throw new System.NotImplementedException();
    }

    public override void OnEntered()
    {
        machine.Controller.OnStep += GoToPlayer;
    }

    public override void OnExited()
    {
        machine.Controller.OnStep -= GoToPlayer;
    }

    public override void Update()
    {
    }

    /// <summary>
    /// trouve le noeud sur lequel se trouve le joueur et recalcule le chemin si il a chang�.
    /// </summary>
    void GoToPlayer()
    {
        Node PlayerNode = machine.Sensor.GetPlayerNode();
        if (PlayerNode != null && (PlayerNode != _currentTarget || _currentTarget == null))
        {
            _currentTarget = PlayerNode;
            machine.Controller.SetDestination(_currentTarget);
        }
    }

}

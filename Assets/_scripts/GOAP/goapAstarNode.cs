using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class goapAstarNode : AstarNode
{
    StateBase AssociatedState;
    GOAP engine;
    public GameContext SimulatedOutcome;


    public goapAstarNode(StateBase state, GOAP goap)
    {
        AssociatedState = state;
        this.engine = goap;
        OnOpened += UpdateSimulatedOutcome;
    }

    
    public override float ComputeCost(AstarNode target)
    {
        //son but est de minimiser les HPs du joueur tout en maximisant les siens.
        //le cout de chaque noeud dépend donc de la différence entre les deux.
        //si le contexte simulé du noeud précédent ne permet pas de rentrer dans ce noeud-ci, alors le cout est infini.
        if (AssociatedState.CanBeEnteredFromContext(((goapAstarNode)precedentNode).SimulatedOutcome))
        {
            return SimulatedOutcome.AgentHp - SimulatedOutcome.PlayerHp;
        }
        else return Mathf.Infinity;
    }


    public override bool isActive()
    {
        return true;
    }


    public void UpdateSimulatedOutcome()
    {
        if(precedentNode != null) SimulatedOutcome = AssociatedState.SimulateOutcome(((goapAstarNode)precedentNode).SimulatedOutcome);
    }

}

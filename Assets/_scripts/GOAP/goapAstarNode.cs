using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


[Serializable]
public class goapAstarNode : AstarNode
{
    [HideInInspector] public StateBase State;
    [HideInInspector] public GoapEngine Engine;
    public GameContext SimulatedOutcomeContext;
    public GoapNodeVisualizer Visualizer;

    public goapAstarNode(StateBase state, GoapEngine goap, GoapNodeVisualizer visu)
    {
        State = state;
        this.Engine = goap;

        Visualizer = visu;
        visu.Node = this;

        OnBeforeActivationCheck += UpdateSimulatedOutcome;
    }

    public override bool isActive()
    {
        if (previousNode == null) { return State.CanBeEnteredFromContext(SimulatedOutcomeContext); }
        return State.CanBeEnteredFromContext(((goapAstarNode)previousNode).SimulatedOutcomeContext) ;
    }

    public override float ComputeCost(AstarNode target)
    {
        //son but est de minimiser les HPs du joueur tout en maximisant les siens.
        //le cout de chaque noeud dépend donc de la différence entre les deux.
        return   SimulatedOutcomeContext.PlayerHp - SimulatedOutcomeContext.AgentHp;
    }

    public void UpdateSimulatedOutcome()
    {
        if (previousNode != null)
        {
            //chaque noeud simule son contexte de sortie à partir du contexte du noeud precedent
            SimulatedOutcomeContext = State.SimulateOutcomeContext(((goapAstarNode)previousNode).SimulatedOutcomeContext);
        } 
        else
        {
            //le noeud de départ récupère le vrai contexte du jeu
            SimulatedOutcomeContext = Engine.GetCurrentGameContext();
        }
        Visualizer.UpdateUI();
    }

}

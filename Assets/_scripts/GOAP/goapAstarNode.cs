using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[Serializable]
public class goapAstarNode : AstarNode
{
    public StateBase State;
    public GOAP engine;
    public GameContext SimulatedOutcomeContext { get { return SimulatedOutcomeContext; } set { SimulatedOutcomeContext = value; Visu.UpdateUI(); } }
    public GoapNodeVisualizer Visu;

    public goapAstarNode(StateBase state, GOAP goap, GoapNodeVisualizer visu)
    {
        State = state;
        this.engine = goap;

        Visu = visu;
        visu.Node = this;

        OnBeforeActivationCheck += UpdateSimulatedOutcome;
        //OnBeforeActivationCheck += ;
    }
    public override bool isActive()
    {
        //si le contexte simul� du noeud pr�c�dent ne permet pas de rentrer dans ce noeud-ci, alors le cout est infini
        return previousNode ==null || State.CanBeEnteredFromContext(((goapAstarNode)previousNode).SimulatedOutcomeContext) ;
    }

    public override float ComputeCost(AstarNode target)
    {
        //son but est de minimiser les HPs du joueur tout en maximisant les siens.
        //le cout de chaque noeud d�pend donc de la diff�rence entre les deux.
        return SimulatedOutcomeContext.AgentHp - SimulatedOutcomeContext.PlayerHp;
    }

    public void UpdateSimulatedOutcome()
    {
        if (previousNode != null)
        {
            //chaque noeud simule son contexte de sortie � partir du contexte du noeud precedent
            SimulatedOutcomeContext = State.SimulateOutcomeContext(((goapAstarNode)previousNode).SimulatedOutcomeContext);
        } 
        else
        {
            //le noeud de d�part r�cup�re le vrai contexte du jeu
            SimulatedOutcomeContext = State.SimulateOutcomeContext(engine.GetCurrentGameContext());
        } 
    }

}

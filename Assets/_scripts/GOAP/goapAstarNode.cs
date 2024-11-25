using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class goapAstarNode : AstarNode
{
    StateBase State;
    GOAP engine;
    public GameContext SimulatedOutcomeContext;


    public goapAstarNode(StateBase state, GOAP goap)
    {
        State = state;
        this.engine = goap;
        OnOpened += UpdateSimulatedOutcome;
    }
    public override bool isActive()
    {
        return true;
    }

    public override float ComputeCost(AstarNode target)
    {
        //son but est de minimiser les HPs du joueur tout en maximisant les siens.
        //le cout de chaque noeud d�pend donc de la diff�rence entre les deux.
        //si le contexte simul� du noeud pr�c�dent ne permet pas de rentrer dans ce noeud-ci, alors le cout est infini.
        if (State.CanBeEnteredFromContext(((goapAstarNode)precedentNode).SimulatedOutcomeContext))
        {
            return SimulatedOutcomeContext.AgentHp - SimulatedOutcomeContext.PlayerHp;
        }
        else return Mathf.Infinity;
    }

    public void UpdateSimulatedOutcome()
    {
        if (precedentNode != null)
        {
            //chaque noeud simule son contexte de sortie � partir du contexte du noeud precedent
            SimulatedOutcomeContext = State.SimulateOutcomeContext(((goapAstarNode)precedentNode).SimulatedOutcomeContext);
        } 
        else
        {
            //le noeud de d�part r�cup�re le vrai contexte du jeu
            SimulatedOutcomeContext = engine.GetCurrentGameContext();
        } 
    }

}

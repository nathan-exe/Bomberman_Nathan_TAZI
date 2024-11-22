using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class State_Win : StateBase
{
    public State_Win(StateMachine sm) : base(sm) { }

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
        Time.timeScale = 0;
        Debug.Log("l'IA a gagné");
    }

    public override void OnExited()
    {
    }

    public override void Update()
    {
    }
}

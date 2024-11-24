using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class State_Dead : StateBase
{
    public State_Dead(StateMachine sm) : base(sm) { }

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
    }

    public override void OnExited()
    {
    }

    public override void Update()
    {
    }
}

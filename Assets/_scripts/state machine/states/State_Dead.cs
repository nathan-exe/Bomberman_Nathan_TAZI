using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Dead : StateBase
{
    public State_Dead(StateMachine sm) : base(sm)
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
        throw new System.NotImplementedException();
    }

    public override void OnExited()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}

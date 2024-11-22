using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class State_FleeingBomb : StateBase
{
    public State_FleeingBomb(StateMachine sm) : base(sm)
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

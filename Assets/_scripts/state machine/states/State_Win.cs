using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class State_Win : StateBase
{
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
        Debug.Log("GG");
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

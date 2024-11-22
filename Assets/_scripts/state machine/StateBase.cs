using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateBase
{
    //Astar
    public StateOutcome Outcome;
    public StateConditions Conditions;

    protected StateMachine machine;
    StateConditions cond = new StateConditions();
    public abstract void OnEntered();
    public abstract void OnExited();
    public abstract void Update();

    public StateBase(StateMachine sm)
    {
        machine = sm;
    }

    public void transitionTo(StateBase nextState)
    {
        machine.transitionTo( nextState);
    }

    public abstract float computeScore(StateOutcome outcome, StateWeights weights);

    public abstract bool canTransitionToState(StateConditions conditions);

}


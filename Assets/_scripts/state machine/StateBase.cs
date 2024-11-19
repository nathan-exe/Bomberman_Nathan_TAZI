using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateBase
{
    //Astar
    public StateOutcome Outcome;
    public StateConditions Conditions;

    StateMachine machine;
    StateConditions cond = new StateConditions();
    public abstract void OnEntered();
    public abstract void OnExited();
    public abstract void Update();

    public void Init(StateMachine sm)
    {
        machine = sm;
    }

    public void transitionTo(StateBase nextState)
    {
        machine.transitionTo( nextState);
    }

    


    float computeScore(StateOutcome outcome,StateWeights weights)
    {
        
    }

    bool canTransitionToState(StateConditions conditions)
    {
         
    }
}


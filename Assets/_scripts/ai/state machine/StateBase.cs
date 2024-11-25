using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateBase
{

    protected StateMachine machine;
    public abstract void OnEntered();
    public abstract void OnExited();
    public abstract void Update();

    public StateBase(StateMachine sm)
    {
        machine = sm;
    }

    public void transitionTo(StateBase nextState)
    {
        machine.transitionTo(nextState);
    }


    public abstract GameContext SimulateOutcomeContext(GameContext precedentContext);
    public abstract bool CanBeEnteredFromContext(GameContext precedentContext);

}
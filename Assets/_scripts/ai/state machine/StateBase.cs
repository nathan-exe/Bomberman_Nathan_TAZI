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

    /// <summary>
    /// simule les changements que cet �tat apportera au contexte donn�.
    /// </summary>
    /// <param name="precedentContext"></param>
    /// <returns></returns>
    public abstract GameContext SimulateOutcomeContext(GameContext precedentContext);

    /// <summary>
    /// v�rifie si l'on peut rentrer dans cet �tat depuis le contexte donn�.
    /// </summary>
    /// <param name="precedentContext"></param>
    /// <returns></returns>
    public abstract bool CanBeEnteredFromContext(GameContext precedentContext);

}
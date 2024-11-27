using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class State_Dead : StateBase
{
    public State_Dead(StateMachine sm) : base(sm) { }

    public override bool CanBeEnteredFromContext(GameContext precedentContext)
    {
        return precedentContext.AgentHp <= 0;
    }
    public override GameContext SimulateOutcomeContext(GameContext precedentContext)
    {
        return precedentContext; 
    }

    public override void OnEntered()
    {
        Time.timeScale = 0;
        GameOver.Instance.triggerPlayerWin();
    }

    public override void OnExited()
    {
    }


    public override void Update()
    {
    }
}

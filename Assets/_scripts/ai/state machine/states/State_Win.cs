using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class State_Win : StateBase
{
    public State_Win(StateMachine sm) : base(sm) { }

    public override bool CanBeEnteredFromContext(GameContext ctx)
    {
        return ctx.AgentHp>0 && ctx.PlayerHp <= 0;
    }

    public override GameContext SimulateOutcomeContext(GameContext precedentContext)
    {
        return precedentContext;
    }

    public override void OnEntered()
    {
        Time.timeScale = 0;
        Debug.Log("l'IA a gagn�");
    }

    public override void OnExited()
    {
    }

    

    public override void Update()
    {
    }
}

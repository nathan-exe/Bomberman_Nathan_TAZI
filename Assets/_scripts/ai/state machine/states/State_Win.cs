using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class State_Win : StateBase
{
    public State_Win(StateMachine sm) : base(sm) { }

    public override void OnEntered()
    {
        Time.timeScale = 0;
        Debug.Log("l'IA a gagné");
    }

    public override void OnExited()
    {
    }

    public override void Update()
    {
    }
}

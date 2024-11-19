using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    StateBase currentState;

    //states
    
    private void Awake()
    {
        //state.init()
    }

    public void transitionTo(StateBase to)
    {
        currentState.OnExited();
        currentState = to;
        to.OnEntered();
    }
}

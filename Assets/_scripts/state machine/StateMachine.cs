using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    StateBase currentState;

    //states

    //references
    public Astar Pathfinder;
    public Move Movement;
    public AIsensor Sensor;

    private void Awake()
    {
        
    }

    public void transitionTo(StateBase to)
    {
        currentState.OnExited();
        currentState = to;
        to.OnEntered();
    }


    private void Update()
    {
        currentState.Update();
    }
}

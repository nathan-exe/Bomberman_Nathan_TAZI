using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

/// <summary>
/// la machine � �tat du bot.
/// </summary>
public class StateMachine : MonoBehaviour
{

    StateBase currentState;

    //states
    public State_ChasingPlayer S_ChasingPlayer;
    public State_FleeingBomb S_FleeingBomb;
    public State_CollectingBombs S_CollectingBombs;
    public State_Dead S_Dead;
    public State_Win S_Win;

    //references
    public AiSensor Sensor;
    public AiController Controller;


    private void Awake()
    {
        InitStates();
    }

    private IEnumerator Start()
    {
        yield return 0 ;
        transitionTo(S_CollectingBombs);
    }

    /// <summary>
    /// transitionne vers un autre �tat
    /// </summary>
    /// <param name="to"></param>
    public void transitionTo(StateBase to)
    {
        if(currentState!=null) currentState.OnExited();
        currentState = to;
        currentState.OnEntered();
    }

    private void Update()
    {
        if(currentState!=null) currentState.Update();
    }

    /// <summary>
    /// instancie tous les etats
    /// </summary>
    void InitStates()
    {
         S_ChasingPlayer = new(this);
         S_FleeingBomb = new(this);
         S_CollectingBombs = new(this);
         S_Dead = new(this);
         S_Win = new(this);
    }
}

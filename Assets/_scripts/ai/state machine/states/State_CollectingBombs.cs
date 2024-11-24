using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class State_CollectingBombs : StateBase
{
    AstarNode _currentTarget;
    public State_CollectingBombs(StateMachine sm) : base(sm) { }

    public override bool canTransitionToState(StateConditions conditions)
    {
        throw new System.NotImplementedException();
    }

    public override float computeScore(StateOutcome outcome, StateWeights weights)
    {
        throw new System.NotImplementedException();
    }

    public override void OnEntered()
    {
        FindPathToNearestBomb();

        machine.Sensor.OnBombPickedUpByPlayer += FindPathToNearestBomb;
        machine.Sensor.OnBombPickedUpByAgent += FindPathToNearestBomb;
    }

    public override void OnExited()
    {
        machine.Sensor.OnBombPickedUpByPlayer -= FindPathToNearestBomb;
        machine.Sensor.OnBombPickedUpByAgent -= FindPathToNearestBomb;
    }

    public override void Update()
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// trouve la bombe la plus proche et recalcule le chemin vers elle si elle a changé
    /// </summary>
    void FindPathToNearestBomb()
    {
        AstarNode nearestBomb = machine.Sensor.FindNearestBombNode();
        if (nearestBomb  != null && (nearestBomb != _currentTarget || _currentTarget == null))
        {
            _currentTarget = nearestBomb;
            machine.Controller.SetDestination(_currentTarget);
        }
    }
}

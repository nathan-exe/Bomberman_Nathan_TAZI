using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class State_FleeingBomb : StateBase
{
    private List<Bomb> _nearbyBombs = new();
    public State_FleeingBomb(StateMachine sm) : base(sm)
    {
    }

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
        machine.Sensor.OnAgentMoved += ReactToNearbyBombs;
        machine.Sensor.OnBombPlacedByAgent += ReactToNearbyBombs;
        machine.Sensor.OnBombPlacedByPlayer += ReactToNearbyBombs;
    }

    public override void OnExited()
    {
        machine.Sensor.OnAgentMoved -= ReactToNearbyBombs;
        machine.Sensor.OnBombPlacedByAgent -= ReactToNearbyBombs;
        machine.Sensor.OnBombPlacedByPlayer -= ReactToNearbyBombs;
    }

    public override void Update()
    {
    }

    public void ReactToNearbyBombs()
    {
        //si il est dans le rayon d'action d'une bombe
        machine.Sensor.FindTickingBombsAroundPoint_NoAlloc(machine.transform.position.Round(),ref _nearbyBombs);
        if (_nearbyBombs.Count > 0) 
        {
            //il se déplace vers une case libre et safe aléatoire
            List<Vector2Int> freeTiles = machine.Sensor.FindFreeNodesAroundPoint(machine.transform.position);
            Debug.Log(freeTiles.Count);

            for (int i = 0; i < freeTiles.Count; i++) 
            {
                if (!machine.Sensor.IsTileSafe(freeTiles[i]))
                {
                    freeTiles.RemoveAt(i);
                    i--;
                }
            }
            Debug.Log(freeTiles.Count);

            if (freeTiles.Count > 0) machine.Controller.SetDestination(Graph.Instance.Nodes[freeTiles[Random.Range(0, freeTiles.Count)]]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOAP : MonoBehaviour
{

    [SerializeField] StateMachine stateMachine;
    [SerializeField] AiSensor sensor;

    public goapAstarNode CollectingBombs;
    public goapAstarNode FleeingBombs;
    public goapAstarNode ChasingPlayer;

    Astar astar;

    goapAstarNode currentNode;

    private void Awake()
    {
        InstantiateStates();
        LinkStates();
    }

    void InstantiateStates()
    {
        CollectingBombs = new(stateMachine.S_CollectingBombs, this);
        FleeingBombs = new(stateMachine.S_FleeingBomb, this);
        ChasingPlayer = new(stateMachine.S_ChasingPlayer, this);
    }

    void LinkStates()
    {
        CollectingBombs.Neighbours = new List<AstarNode>() {FleeingBombs,ChasingPlayer};
        ChasingPlayer.Neighbours = new List<AstarNode>() {CollectingBombs,FleeingBombs};
    }

    GameContext GetCurrentGameState()
    {
        return new(); //@temp
    }

    public Stack<AstarNode> FindBestPath()
    {
        currentNode.SimulatedOutcome = GetCurrentGameState();
        return astar.ComputePath(currentNode,Win);
    }
}

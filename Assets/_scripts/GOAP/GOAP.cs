using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOAP : MonoBehaviour
{

    [SerializeField] StateMachine stateMachine;
    [SerializeField] AiSensor sensor;
    [SerializeField] Astar astar;

    public goapAstarNode CollectingBombs, FleeingBombs, ChasingPlayer, Win, Death;

    goapAstarNode currentNode;

    [Header("Tests values")]
    GameContext ctx;

    private void Awake()
    {
        InstantiateNodes();
        LinkNodes();
    }

    void InstantiateNodes()
    {
        CollectingBombs = new(stateMachine.S_CollectingBombs, this);
        FleeingBombs = new(stateMachine.S_FleeingBomb, this);
        ChasingPlayer = new(stateMachine.S_ChasingPlayer, this);
        Death = new(stateMachine.S_ChasingPlayer, this);
        Win = new(stateMachine.S_ChasingPlayer, this);
    }

    void LinkNodes()
    {
        CollectingBombs.Neighbours = new List<AstarNode>() { FleeingBombs,ChasingPlayer,Death,Win };
        ChasingPlayer.Neighbours = new List<AstarNode>() { CollectingBombs, FleeingBombs,Death, Win };
        FleeingBombs.Neighbours = new List<AstarNode>() { CollectingBombs, ChasingPlayer, Death, Win };
        Win.Neighbours = new List<AstarNode>() { CollectingBombs, FleeingBombs, ChasingPlayer, Death };
        Death.Neighbours = new List<AstarNode>() { CollectingBombs, FleeingBombs, ChasingPlayer, Win };
    }

    void ResetAllNodes()
    {
        CollectingBombs.resetNode();
        FleeingBombs.resetNode();
        ChasingPlayer.resetNode();
        Death.resetNode();
        Win.resetNode();
    }

    public GameContext GetCurrentGameContext()
    {
        return ctx; //@temp
    }

    public Stack<AstarNode> FindBestPath()
    {
        ResetAllNodes();
        //currentNode.SimulatedOutcomeContext = GetCurrentGameContext();
        return astar.ComputePath(currentNode,Win);
    }
}

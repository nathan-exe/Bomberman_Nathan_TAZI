using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GOAP : MonoBehaviour
{

    [SerializeField] StateMachine stateMachine;
    [SerializeField] AiSensor sensor;
    [SerializeField] Astar astar;

    public goapAstarNode CollectingBombs, FleeingBombs, ChasingPlayer, Win, Death;
    public GoapNodeVisualizer Visu_CollectingBombs, Visu_FleeingBombs, Visu_ChasingPlayer, Visu_Win, Visu_Death;

    public goapAstarNode  CurrentNode { get; private set; }

    [Header("Tests values")]
    [SerializeField] GameContext ctx;

    

    private void Awake()
    {
        InstantiateNodes();
        LinkNodes();
        CurrentNode = CollectingBombs;
    }

    void InstantiateNodes()
    {
        CollectingBombs = new(stateMachine.S_CollectingBombs, this,Visu_CollectingBombs);
        FleeingBombs = new(stateMachine.S_FleeingBomb, this,Visu_FleeingBombs);
        ChasingPlayer = new(stateMachine.S_ChasingPlayer, this,Visu_ChasingPlayer);
        Death = new(stateMachine.S_Dead, this,Visu_Death);
        Win = new(stateMachine.S_Win, this,Visu_Win);
    }

    void LinkNodes()
    {
        CollectingBombs.Neighbours = new List<AstarNode>() { FleeingBombs,ChasingPlayer,Death,Win };
        ChasingPlayer.Neighbours = new List<AstarNode>() { CollectingBombs, FleeingBombs,Death, Win };
        FleeingBombs.Neighbours = new List<AstarNode>() { CollectingBombs, ChasingPlayer, Death, Win };
        Win.Neighbours = new List<AstarNode>() {};
        Death.Neighbours = new List<AstarNode>() {};
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
        CurrentNode.UpdateSimulatedOutcome();

        Stack<AstarNode> path = astar.ComputePath(CurrentNode,Win); //itérer plusieurs fois sans reset les nodes ? while !path.contains(win) ?
        print(path.Count);
        if(path.Count>0) ((goapAstarNode)path.Peek()).Visu.SetRed();
        while (path.Count > 0) Debug.Log(((goapAstarNode)path.Pop()).State.GetType());
        return null;
    }
}


[CustomEditor(typeof(GOAP))]
class GOAPeditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Test")) ((GOAP)target).FindBestPath();
    }
}

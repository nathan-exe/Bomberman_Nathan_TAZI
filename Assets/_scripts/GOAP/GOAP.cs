using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class GOAP : MonoBehaviour
{

    [SerializeField] StateMachine stateMachine;
    [SerializeField] AiSensor sensor;
    [SerializeField] Astar astar;

    public goapAstarNode CollectingBombs, FleeingBombs, ChasingPlayer, Win, Death;

    [Header("Visuals")]
    [SerializeField] GoapNodeVisualizer Visu_CollectingBombs;
    [SerializeField] GoapNodeVisualizer Visu_FleeingBombs, Visu_ChasingPlayer, Visu_Win, Visu_Death;

    public goapAstarNode  CurrentNode { get; private set; }

    private void Start()
    {
        InstantiateNodes();
        LinkNodes();
        CurrentNode = CollectingBombs;
        SetUpEvents();
    }

    public void SetUpEvents()
    {
        sensor.OnBombPickedUpByAgent += OnContextChanged;
        sensor.OnBombPickedUpByPlayer += OnContextChanged;
        sensor.OnBombPlacedByAgent += OnContextChanged;
        sensor.OnBombPlacedByPlayer += OnContextChanged;
        sensor.OnAgentHealthUpdated += OnContextChanged;
        sensor.OnPlayerHealthUpdated += OnContextChanged;
        //onBombExploded
    }

    void OnContextChanged()
    {
        chooseBestAction();
    }

    void chooseBestAction()
    {
        Stack<AstarNode> path = FindBestPath();
        if (path.Count > 0)
        {
            CurrentNode = (goapAstarNode)path.Pop();
            stateMachine.transitionTo(CurrentNode.State);
        }
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
        GameContext ctx = new();
        ctx.PlayerHp = sensor.PlayerHP;
        ctx.AgentHp = sensor.AgentHP;
        ctx.AgentBombCount = sensor.AgentBombCount;
        ctx.PlayerBombCount = sensor.PlayerBombCount;

        ctx.DangerousBombsAroundAgent = sensor.FindTickingBombsAroundPoint((Vector2)sensor.AgentPosition).Count;
        ctx.DangerousBombsAroundPlayer = sensor.FindTickingBombsAroundPoint((Vector2)sensor.AgentPosition).Count;

        return ctx; 
    }

    public Stack<AstarNode> FindBestPath()
    {
        ResetAllNodes();
        CurrentNode.UpdateSimulatedOutcome();
        Stack<AstarNode> path = astar.ComputePath(CurrentNode, Win);
        if(path.Count>0) ((goapAstarNode)path.ToArray()[path.Count - 1]).Visu.SetRed();
        return path;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(GOAP))]
class GOAPeditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GOAP t = (GOAP)target;
        if (GUILayout.Button("Test")) t.FindBestPath();
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/// <summary>
/// goal oriented action planning
/// </summary>
public class GoapEngine : MonoBehaviour
{

    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private AiSensor sensor;
    [SerializeField] private Astar astar;

    //nodes
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
        sensor.OnAgentMoved += OnContextChanged;
        sensor.OnPlayerMoved += OnContextChanged;
    }

    void OnContextChanged()
    {
        chooseBestAction();
    }

    /// <summary>
    /// choisit la meilleure action à faire en fonction du contexte du jeu, 
    /// et fais transitionner la state machine vers l'etat correspondant.
    /// </summary>
    void chooseBestAction()
    {
        Stack<AstarNode> path = FindBestPath();
        if (path.Count > 0)
        {
            CurrentNode = (goapAstarNode)path.Pop();
            stateMachine.transitionTo(CurrentNode.State);
            //EditorApplication.isPaused = true;
        }
    }

    /// <summary>
    /// instantie tous les noeuds du graph
    /// </summary>
    void InstantiateNodes()
    {
        CollectingBombs = new(stateMachine.S_CollectingBombs, this,Visu_CollectingBombs);
        FleeingBombs = new(stateMachine.S_FleeingBomb, this,Visu_FleeingBombs);
        ChasingPlayer = new(stateMachine.S_ChasingPlayer, this,Visu_ChasingPlayer);
        Death = new(stateMachine.S_Dead, this,Visu_Death);
        Win = new(stateMachine.S_Win, this,Visu_Win);
    }

    /// <summary>
    /// relie tous les noeuds du graph entre eux
    /// </summary>
    void LinkNodes()
    {
        CollectingBombs.Neighbours = new List<AstarNode>() { FleeingBombs,ChasingPlayer,Death,Win };
        ChasingPlayer.Neighbours = new List<AstarNode>() { CollectingBombs, FleeingBombs,Death, Win };
        FleeingBombs.Neighbours = new List<AstarNode>() { CollectingBombs, ChasingPlayer, Death, Win };
        Win.Neighbours = new List<AstarNode>() {};
        Death.Neighbours = new List<AstarNode>() {};
    }

    /// <summary>
    /// reset tous les noeuds
    /// </summary>
    void ResetAllNodes()
    {
        CollectingBombs.resetNode();
        FleeingBombs.resetNode();
        ChasingPlayer.resetNode();
        Death.resetNode();
        Win.resetNode();
    }

    /// <summary>
    /// retourne le contexte actuel du jeu
    /// </summary>
    /// <returns></returns>
    public GameContext GetCurrentGameContext()
    {
        GameContext ctx = new();
        ctx.PlayerHp = sensor.PlayerHP;
        ctx.AgentHp = sensor.AgentHP;
        ctx.AgentBombCount = sensor.AgentBombCount;
        ctx.PlayerBombCount = sensor.PlayerBombCount;

        ctx.DangerousBombsAroundAgent = sensor.CountTickingBombsAroundPoint((Vector2)sensor.AgentPosition);
        ctx.DangerousBombsAroundPlayer = sensor.CountTickingBombsAroundPoint((Vector2)sensor.AgentPosition);
        return ctx; 
    }

    public Stack<AstarNode> FindBestPath()
    {
        ResetAllNodes();
        CurrentNode.UpdateSimulatedOutcome();

        Stack<AstarNode> path = astar.ComputePath(CurrentNode, Win);
        if(path.Count>0) ((goapAstarNode)path.ToArray()[path.Count - 1]).Visualizer.SetRed();
        
        return path;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(GoapEngine))]
class GOAPeditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GoapEngine t = (GoapEngine)target;
        if (GUILayout.Button("Test")) t.FindBestPath();
    }
}
#endif
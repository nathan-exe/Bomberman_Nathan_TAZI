using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour
{
    public Dictionary<Vector2Int, TileAstarNode> Nodes = new();
    public NodeContainer NodePrefab;

    public List<TileAstarNode> FreeNodes { get; private set; } = new();

    //singleton
    private static Graph _instance;
    public static Graph Instance => _instance;

    public const string NodeLayer = "graph";

    private void Awake()
    {
        if(_instance != null && _instance!=this) Destroy(_instance.gameObject); 
        _instance = this;

        FindObjectOfType<GraphBuilder>().BuildGraph();

        //génération du dictionnaire de noeuds
        Nodes.Clear();
        FreeNodes.Clear();
        foreach(Transform t in transform)
        {
            if (t.gameObject.TryGetComponent<NodeContainer>(out NodeContainer n)) 
            {
                n.node.monoBehaviour = n;
                Nodes.Add(n.node.pose, n.node);
            }
            if (t.gameObject.activeSelf) FreeNodes.Add(n.node);
        }
    }

    //fonctions pratiques
    public void AddNodeToGraph(Vector2Int position)
    {
        Nodes[position].gameObject.SetActive(true);
        FreeNodes.Add(Nodes[position]);
    }

    public bool HasNodeAtPosition(Vector2Int position)
    {
        return Nodes.ContainsKey(position) && Nodes[position].gameObject.activeSelf;
    }

    public void RemoveNodeFromGraph(TileAstarNode node)
    {
        node.gameObject.SetActive(false);
        FreeNodes.Remove(node);
    }

    public void RemoveNodeFromGraph(Vector2Int position)
    {
        if(Nodes.ContainsKey(position)) RemoveNodeFromGraph (Nodes[position]);
    }

}

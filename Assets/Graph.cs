using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour
{
    public Dictionary<Vector2Int,Node> Nodes = new();
    public Node NodePrefab;

    public List<Node> FreeNodes { get; private set; } = new();

    //singleton
    private static Graph _instance;
    public static Graph Instance => _instance;

    public const string NodeLayer = "graph";

    private void Awake()
    {
        if(_instance != null && _instance!=this) Destroy(_instance.gameObject); 
        _instance = this;
        
        //génération du dictionnaire de noeuds
        Nodes.Clear();
        FreeNodes.Clear();
        foreach(Transform t in transform)
        {
            if (t.gameObject.TryGetComponent<Node>(out Node n)) Nodes.Add(n.pose, n);
            if (t.gameObject.activeSelf) FreeNodes.Add(n);
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

    public void RemoveNodeFromGraph(Node node)
    {
        node.gameObject.SetActive(false);
        FreeNodes.Remove(node);
    }

    public void RemoveNodeFromGraph(Vector2Int position)
    {
        if(Nodes.ContainsKey(position)) RemoveNodeFromGraph (Nodes[position]);
    }

}

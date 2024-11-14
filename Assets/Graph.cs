using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour
{
    public Dictionary<Vector2Int,Node> Nodes = new();
    public Node NodePrefab;

    //singleton
    private static Graph _instance;
    public static Graph Instance => _instance;

    private void Awake()
    {
        if(_instance != null && _instance!=this) Destroy(_instance.gameObject); 
        _instance = this; 
    }

    //fonctions pratiques
    public void AddNodeToGraph(Vector2Int position)
    {
        Nodes[position].gameObject.SetActive(true);
    }

    public bool HasNodeAtPosition(Vector2Int position)
    {
        return Nodes.ContainsKey(position) && Nodes[position].gameObject.activeSelf;
    }

    public void RemoveNodeFromGraph(Node node)
    {
        node.gameObject.SetActive(false);
    }

    public void RemoveNodeFromGraph(Vector2Int position)
    {
        if(Nodes.ContainsKey(position)) RemoveNodeFromGraph (Nodes[position]);
    }

}

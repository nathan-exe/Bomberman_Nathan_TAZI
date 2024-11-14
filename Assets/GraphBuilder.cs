using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GraphBuilder : MonoBehaviour
{
    [SerializeField] Vector2Int _size = new Vector2Int(10, 10);
    [SerializeField] Vector2Int _offset = new Vector2Int(10, 10);
    [SerializeField] Graph _graph;

    const string solidLayer = "Solid";
    public void BuildGraph()
    {
        _graph.Nodes.Clear();

        //destroy all children
        int c = transform.childCount;
        for(int i =0; i < c; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        //iterate over Tilemap Area
        for (int x = _offset.x; x < _size.x + _offset.x; x++)
        {
            for (int y= _offset.y; y < _size.y+ _offset.y; y++)
            {
                bool collision = Physics2D.OverlapPoint(new Vector2(x, y), LayerMask.GetMask(solidLayer)); //le node sera desactivé si il y'avait un objet sur la case avant qu'il ne spawn

                Debug.DrawRay(new Vector2(x, y), Vector2.up * 0.2f, Color.red, 1);
                Node newNode = Instantiate(_graph.NodePrefab, new Vector2(x, y), Quaternion.identity, transform);
                newNode.gameObject.name = $"Node({x},{y})";
                _graph.Nodes.Add(new Vector2Int(x, y), newNode); //add nodes to free tiles

                newNode.gameObject.SetActive(!collision);
            }
        }

        //link nodes with eachother
        foreach (KeyValuePair<Vector2Int,Node> pair in _graph.Nodes)
        {
            Vector2Int pose = pair.Key+ Vector2Int.up;
            if(_graph.Nodes.ContainsKey(pose)) pair.Value.Neighbours.Add(_graph.Nodes[pose]);

            pose = pair.Key + Vector2Int.down;
            if (_graph.Nodes.ContainsKey(pose)) pair.Value.Neighbours.Add(_graph.Nodes[pose]);

            pose = pair.Key + Vector2Int.right;
            if (_graph.Nodes.ContainsKey(pose)) pair.Value.Neighbours.Add(_graph.Nodes[pose]);

            pose = pair.Key + Vector2Int.left;
            if (_graph.Nodes.ContainsKey(pose)) pair.Value.Neighbours.Add(_graph.Nodes[pose]);
        }

    }

    //gizmos
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector3)(Vector2)(_offset+_size/2- Vector2.one*0.5f), (Vector3)(Vector2)(_size));
    }
}

[CustomEditor(typeof(GraphBuilder))]
public class GraphBuilder_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Rebuild Graph"))
        {
            ((GraphBuilder)target).BuildGraph();
        }
    }
}

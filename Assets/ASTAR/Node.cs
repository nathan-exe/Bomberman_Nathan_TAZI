using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum NodeState { open, closed, notVisitedYet }

public class Node : MonoBehaviour
{
    public List<Node> Neighbours = new();
    public Vector2Int pose => transform.position.ToVector2Int();

    //algo
    [HideInInspector] public NodeState state = NodeState.notVisitedYet;
    Node precedentNode = null;

    public static float gWheight = .5f;
    [HideInInspector] public int g;
    [HideInInspector] public float h;
    public float f => h + g* gWheight;

    //visuels
    SpriteRenderer _spriteRenderer;
    const bool _debugView = false;

    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        resetNode();
    }

    /// <summary>
    /// remet le noeud à 0 : non visité et blanc
    /// </summary>
    public void resetNode()
    {
        if(_debugView) _spriteRenderer.color = Color.white;
        g = 0;
        h = 0;
        state = NodeState.notVisitedYet;
        precedentNode = null;
    }

    /// <summary>
    /// ouvre le noeud
    /// </summary>
    public void open()
    {
        if (_debugView) _spriteRenderer.color = Color.green;
        state = NodeState.open;
    }

    /// <summary>
    /// ouvre le noeud et sauvegarde le noeud précédent
    /// </summary>
    /// <param name="node"></param>
    public void openFromNode(Node node)
    {
        precedentNode = node;
        open();
    }

    /// <summary>
    /// ferme le noeud
    /// </summary>
    public void close()
    {
        if (_debugView) _spriteRenderer.color = Color.red;
        state = NodeState.closed;
    }

    //algo
    /// <summary>
    /// se ferme et ouvre tous ses voisins
    /// </summary>
    /// <param name="openNodes"></param>
    /// <param name="target"></param>
    public void parcourir(ref List<Node> openNodes,Node target)
    {
        close();
        openNodes.Remove(this);

        foreach (Node n in Neighbours)
        {
            if (n.isActiveAndEnabled && n.state == NodeState.notVisitedYet)
            {
                openNodes.Add(n);
                n.openFromNode(this);
                n.h = Vector2.Distance(target.transform.position, n.transform.position);
                n.g = g + 1;
            }
        }
    }

    /// <summary>
    /// retrouve le chemin vers le premier noeud de manière récursive
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    public Stack<Node> findPathToBeginning(Stack<Node> l)
    {
        l.Push(this);
        if (precedentNode == null) return l;
        Debug.DrawLine(transform.position, precedentNode.transform.position,Color.red,5);
         return precedentNode.findPathToBeginning(l);
    }


    //----------- Guizmos -----------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach (Node node in Neighbours)
        {
            if(node.isActiveAndEnabled)   Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, .5f, 1, .3f);
        foreach (Node node in Neighbours)
        {
            if (node.isActiveAndEnabled)  Gizmos.DrawLine(transform.position, node.transform.position);
        }

    }

}

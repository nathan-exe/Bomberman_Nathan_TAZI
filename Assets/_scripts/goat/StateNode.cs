using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode
{
    public List<StateNode> Neighbours = new();
    //algo
    [HideInInspector] public NodeState state = NodeState.notVisitedYet;
    StateNode precedentNode = null;

    private void Awake()
    {
        resetNode();
    }

    /// <summary>
    /// remet le noeud � 0 : non visit� et blanc
    /// </summary>
    public void resetNode()
    {
        state = NodeState.notVisitedYet;
        precedentNode = null;
    }

    /// <summary>
    /// ouvre le noeud
    /// </summary>
    public void open()
    {
        state = NodeState.open;
    }

    /// <summary>
    /// ouvre le noeud et sauvegarde le noeud pr�c�dent
    /// </summary>
    /// <param name="node"></param>
    public void openFromNode(StateNode node)
    {
        precedentNode = node;
        open();
    }

    /// <summary>
    /// ferme le noeud
    /// </summary>
    public void close()
    {
        state = NodeState.closed;
    }

    //algo
    /// <summary>
    /// se ferme et ouvre tous ses voisins
    /// </summary>
    /// <param name="openNodes"></param>
    /// <param name="target"></param>
    public void parcourir(ref List<StateNode> openNodes, StateNode target)
    {
        close();
        openNodes.Remove(this);

        foreach (StateNode n in Neighbours)
        {
            if (n.state == NodeState.notVisitedYet)
            {
                openNodes.Add(n);
                n.openFromNode(this);
                //calcul score
            }
        }
    }

    /// <summary>
    /// retrouve le chemin vers le premier noeud de mani�re r�cursive
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    public Stack<StateNode> findPathToBeginning(Stack<StateNode> l)
    {
        l.Push(this);
        if (precedentNode == null) return l;
        return precedentNode.findPathToBeginning(l);
    }

    public float computeScore()
    {
        return 0;
    }
}

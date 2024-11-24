using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum NodeState { open, closed, notVisitedYet }

/// <summary>
/// cette classe abstraite repr�sente n'importe quel noeud pouvant �tre parcouru par l'algo Astar.
/// </summary>
[Serializable]
public abstract class AstarNode
{
    [SerializeField] public List<AstarNode> Neighbours = new();

    //algo
    [HideInInspector] public NodeState state = NodeState.notVisitedYet;
    AstarNode precedentNode = null;

    /// <summary>
    /// le nombre de noeuds parcourus depuis le d�but du chemin
    /// </summary>
    [HideInInspector] public int g; 

    /// <summary>
    /// le cout du noeud ind�pendamment de g
    /// </summary>
    [HideInInspector] public float h;
    public abstract float ComputeCost();
    public abstract bool isActive();

    /// <summary>
    /// utilis� pour calculer la variable H du noeud (voir pdf Astar)
    /// H repr�sente une partie du cout du noeud
    /// </summary>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public abstract float compute_h(AstarNode targetNode);


    /// <summary>
    /// remet le noeud � 0 : non visit� et blanc
    /// </summary>
    public void resetNode()
    {
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
        state = NodeState.open;
    }

    /// <summary>
    /// ouvre le noeud et sauvegarde le noeud pr�c�dent
    /// </summary>
    /// <param name="node"></param>
    public void openFromNode(AstarNode node)
    {
        precedentNode = node;
        open();
    }

    /// <summary>
    /// ferme le noeud
    /// </summary>
    public void Close()
    {
        state = NodeState.closed;
    }

    //algo
    /// <summary>
    /// se ferme et ouvre tous ses voisins
    /// </summary>
    /// <param name="openNodes"></param>
    /// <param name="target"></param>
    public void parcourir(ref List<AstarNode> openNodes,AstarNode target)
    {
        Close();
        openNodes.Remove(this);

        foreach (AstarNode n in Neighbours)
        {
            if (n.isActive() && n.state == NodeState.notVisitedYet)
            {
                openNodes.Add(n);
                n.openFromNode(this);
                n.h = n.compute_h(target);
                n.g = g + 1;
            }
        }
    }

    /// <summary>
    /// retrouve le chemin vers le premier noeud de mani�re r�cursive
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    public Stack<AstarNode> findPathToBeginning(Stack<AstarNode> l)
    {
        if (precedentNode == null) return l;
        l.Push(this);
        return precedentNode.findPathToBeginning(l);
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum NodeState { open, closed, notVisitedYet }

/// <summary>
/// cette classe abstraite représente n'importe quel noeud pouvant être parcouru par l'algo Astar.
/// </summary>
[Serializable]
public abstract class AstarNode
{
    [SerializeField] public List<AstarNode> Neighbours = new();

    //algo
    [HideInInspector] public NodeState state = NodeState.notVisitedYet;
    AstarNode precedentNode = null;

    /// <summary>
    /// le nombre de noeuds parcourus depuis le début du chemin
    /// </summary>
    [HideInInspector] public int g; 

    /// <summary>
    /// le cout du noeud indépendamment de g
    /// </summary>
    [HideInInspector] public float h;
    public abstract float ComputeCost();
    public abstract bool isActive();

    /// <summary>
    /// utilisé pour calculer la variable H du noeud (voir pdf Astar)
    /// H représente une partie du cout du noeud
    /// </summary>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public abstract float compute_h(AstarNode targetNode);


    /// <summary>
    /// remet le noeud à 0 : non visité et blanc
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
    /// ouvre le noeud et sauvegarde le noeud précédent
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
    /// retrouve le chemin vers le premier noeud de manière récursive
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

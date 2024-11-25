using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;

public class Astar : PathFinder
{
    private AstarNode[] _nodes;
    [SerializeField] Transform _graph;

    private void Awake()
    {
        List<TileAstarNode> temp = new();
        foreach (NodeContainer mb in _graph.GetComponentsInChildren<NodeContainer>()) temp.Add(mb.node);
        _nodes = temp.ToArray();

    }

    public override Stack<AstarNode> ComputePath(AstarNode from, AstarNode to)
    {
        if(from == to) return new Stack<AstarNode>();


        ResetNodes();
        List<AstarNode> _openNodes = new();
        AstarNode TargetIfNoPathFound = from; //utilis� si la cible est incaccessible

        //parcourt le premier noeud
        from.parcourir(ref _openNodes,to);
        //tant que la cible n'est pas trouv�e
        while (!_openNodes.Contains(to) && _openNodes.Count>0)
        {
            //trouve le voisin le plus proche
            AstarNode best = _openNodes[0];
            foreach (AstarNode node in _openNodes)
            {
                if(node.ComputeCost(to)< best.ComputeCost(to)) best = node;
                if (node.ComputeCost(to) < TargetIfNoPathFound.ComputeCost(to)) TargetIfNoPathFound = node;
            }
            //parcourt le voisin le plus proche
            best.parcourir(ref _openNodes,to);
        }

        //si la cible est inaccessible, il retourne le chemin vers le noeud le plus proche qu'il ait r�ussi � trouver
        if (_openNodes.Count == 0 && !_openNodes.Contains(to)) return TargetIfNoPathFound.findPathToBeginning(new Stack<AstarNode>());

        //renvoie le chemin complet
        return to.findPathToBeginning(new Stack<AstarNode>());
    }


    public void ResetNodes()
    {
        foreach(AstarNode node in _nodes)
        {
            node.resetNode();
        }
    }



}


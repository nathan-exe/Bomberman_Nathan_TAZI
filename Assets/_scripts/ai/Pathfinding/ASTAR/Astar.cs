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
        //parcourt le premier noeud
        from.parcourir(ref _openNodes,to);
        //tant que la cible n'est pas trouvée
        while (!_openNodes.Contains(to) && _openNodes.Count>0)
        {
            //trouve le voisin le plus proche
            AstarNode best = _openNodes[0];
            foreach (AstarNode node in _openNodes)
            {
                if(node.ComputeCost(to)<best.ComputeCost(to)) best = node;
            }
            //parcourt le voisin le plus proche
            best.parcourir(ref _openNodes,to);
        }

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


using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Astar : PathFinder
{
    [SerializeField] Transform _graph;
    List<AstarNode> _allNodes = new();

    public override Stack<AstarNode> ComputePath(AstarNode from, AstarNode to)
    {
        if(from == to) return new Stack<AstarNode>();

        //reset tous les noeuds du graphe
        if (_allNodes.Count==0) from.AddNeighboursToList_Recursive(ref _allNodes);
        ResetAllNodes(_allNodes);

        List<AstarNode> _openNodes = new();
        AstarNode TargetIfNoPathFound = from; //utilisé si la cible est incaccessible

        //parcourt le premier noeud
        from.parcourir(ref _openNodes,to);
        //tant que la cible n'est pas trouvée
        while (!_openNodes.Contains(to) && _openNodes.Count>0)
        {
            //trouve le voisin le plus proche
            AstarNode best = _openNodes[0];
            foreach (AstarNode node in _openNodes)
            {
                /*print("-");
                print("Node : " + ((goapAstarNode)node).State.GetType().ToString() +" "+ node.ComputeCost(to).ToString());
                print("best : " + ((goapAstarNode)TargetIfNoPathFound).State.GetType().ToString()+ " " + TargetIfNoPathFound.ComputeCost(to).ToString());

                print("-");*/

                if(node.ComputeCost(to)< best.ComputeCost(to)) best = node;
                if (node.ComputeCost(to) < TargetIfNoPathFound.ComputeCost(to)) TargetIfNoPathFound = node;
            }
            //parcourt le voisin le plus proche
            best.parcourir(ref _openNodes,to);
        }

        //si la cible est inaccessible, il retourne le chemin vers le noeud le plus proche qu'il ait réussi à trouver
        if (_openNodes.Count == 0 && !_openNodes.Contains(to)) { /*if((goapAstarNode)TargetIfNoPathFound!=null) print("couldn't reach target, chose " + ((goapAstarNode)TargetIfNoPathFound).State.GetType().ToString()+" instead");*/ return TargetIfNoPathFound.findPathToBeginning(new Stack<AstarNode>()); }

        //renvoie le chemin complet
        // print();
        //print("reached Target " + ((goapAstarNode)to).State.GetType().ToString());
        return to.findPathToBeginning(new Stack<AstarNode>());
    }


    public void ResetAllNodes(List<AstarNode> nodes)
    {
        foreach(AstarNode node in nodes)
        {
            node.resetNode();
        }
    }

}


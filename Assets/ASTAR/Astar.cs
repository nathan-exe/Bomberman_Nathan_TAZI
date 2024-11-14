using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;

public class Astar : MonoBehaviour
{
    private Node[] _nodes;
    [SerializeField] Transform _graph;

    private void Awake()
    {
        _nodes = _graph.GetComponentsInChildren<Node>();
    }

    public  Stack<Node> ComputePath(Node from, Node to)
    {
        if(from == to) return new Stack<Node>();

        ResetNodes();
        List<Node> _openNodes = new();
        //parcourt le premuier noeud
        from.parcourir(ref _openNodes,to);
        //tant que la cible n'est pas trouvée
        while (!_openNodes.Contains(to) && _openNodes.Count>0)
        {
            //trouve le voisin le plus proche
            Node best = _openNodes[0];
            foreach (Node node in _openNodes)
            {
                if(node.f<best.f) best = node;
            }
            //parcourt le voisin le plus proche
            best.parcourir(ref _openNodes,to);
        }

        //renvoie le chemin complet
        return to.findPathToBeginning(new Stack<Node>());
    }


    public void ResetNodes()
    {
        foreach(Node node in _nodes)
        {
            node.resetNode();
        }
    }



}


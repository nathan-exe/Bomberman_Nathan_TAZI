using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goatAstar : MonoBehaviour
{
    private StateNode[] _nodes;

    public Stack<StateNode> ComputePath(StateNode from, StateNode to)
    {
        if (from == to) return new Stack<StateNode>();

        List<StateNode> _openNodes = new();
        //parcourt le premuier noeud
        from.parcourir(ref _openNodes, to);
        //tant que la cible n'est pas trouvée
        while (!_openNodes.Contains(to) && _openNodes.Count > 0)
        {
            //trouve le voisin le plus proche
            StateNode best = _openNodes[0];
            foreach (StateNode node in _openNodes)
            {
                if (node.computeScore() < best.computeScore()) best = node;
            }
            //parcourt le voisin le plus proche
            best.parcourir(ref _openNodes, to);
        }

        //renvoie le chemin complet
        return to.findPathToBeginning(new Stack<StateNode>());
    }

}

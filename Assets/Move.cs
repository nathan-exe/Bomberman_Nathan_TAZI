using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public delegate bool DynamicBoolean();

public class Move : MonoBehaviour
{
    public Node _currentNode { get; private set; }
    public float _moveSpeed;


    /// <summary>
    /// se déplace vers le point donné.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="speed"></param>
    /// <param name="smoothstep"></param>
    /// <returns></returns>
    public async Task MoveToPoint(Node node,float speed,bool smoothstep = true) 
    {
        
        Quaternion startRotation = transform.rotation;
        Vector2 toPoint = (Vector2)transform.position - (Vector2)node.transform.position ;
        Quaternion targetRotation = Quaternion.Euler(0, 0,Mathf.Atan2(toPoint.y,toPoint.x)*Mathf.Rad2Deg+180);

        Vector2 start =transform.position;
        float startTime = Time.time;
        float duration = Vector2.Distance(start, (Vector2)node.transform.position) /speed;
        float endTime = Time.time+duration;

        while(Time.time < endTime)
        {
            if (!Application.isPlaying) return;

            float alpha = Mathf.InverseLerp(startTime, endTime, Time.time);
            if(smoothstep) alpha = Mathf.SmoothStep(0, 1, alpha);

            transform.position = (Vector3) Vector2.Lerp(start, (Vector2)node.transform.position, alpha) + Vector3.forward * -2;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, alpha*1.1f);

            await Task.Yield();
        }

        transform.position = (Vector3)(Vector2)node.transform.position + Vector3.forward * -2;
        transform.rotation = targetRotation;
        _currentNode = node;

    }

    /// <summary>
    /// suit le chemin donné
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="speed"></param>
    /// <param name="smoothstep"></param>
    /// <returns>
    /// retourne true si il a atteint sa destination
    /// </returns>
    public async Task<bool> FollowPath(Stack<Node> nodes, float speed, bool smoothstep = true, DynamicBoolean shouldStop = null)
    {

        while (nodes.Count > 0) //tant qu'il n'a pas atteint sa destination
        {
            await MoveToPoint(nodes.Pop(), speed, smoothstep); //il avance vers le point suivant
            
            if (shouldStop != null && shouldStop()) return false; //le callback peut le faire s'arreter plus tot que prévu si besoin
        }

        return true;

    }

    /// <summary>
    /// trouve le chemin le plus court vers le noeud et fait bouger le bonhomme vers lui.
    /// </summary>
    /// <param name="node"></param>
    /// <returns>retourne true si il a atteint sa destination</returns>
    public async Task<bool> TravelToNodeThroughGraph(Node node, Astar _pathFinder, DynamicBoolean shouldStop = null)
    {
        //trouve le chemin avec le A*
        _pathFinder.ResetNodes();
        Stack<Node> path = _pathFinder.ComputePath(_currentNode, node);
        if (path.Count <= 1) return true;

        //suit le chemin
        bool destinationWasReached = await FollowPath(path, _moveSpeed,true,shouldStop);

        return destinationWasReached;
    }

    /// <summary>
    /// teleports the objet to the given pose, but also snaps it to the grid.
    /// </summary>
    /// <param name="pose"></param>
    public void TeleportToPosition(Vector2 pose)
    {
        transform.position = (Vector2)pose.RoundToInt();
        _currentNode = Graph.Instance.Nodes[pose.RoundToInt()];
    }
}



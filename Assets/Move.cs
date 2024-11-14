using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class Move : MonoBehaviour
{
    [SerializeField] Node _currentNode;
    public float _moveSpeed;


    /// <summary>
    /// se déplace vers le point donné.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="speed"></param>
    /// <param name="smoothstep"></param>
    /// <returns></returns>
    public async Task MoveToPoint(Vector2 point,float speed,bool smoothstep = true) 
    {
        
        Quaternion startRotation = transform.rotation;
        Vector2 toPoint= (Vector2)transform.position-point;
        Quaternion targetRotation = Quaternion.Euler(0, 0,Mathf.Atan2(toPoint.y,toPoint.x)*Mathf.Rad2Deg+180);

        Vector2 start =transform.position;
        float startTime = Time.time;
        float duration = Vector2.Distance(start, point)/speed;
        float endTime = Time.time+duration;

        while(Time.time < endTime)
        {
            float alpha = Mathf.InverseLerp(startTime, endTime, Time.time);
            if(smoothstep) alpha = Mathf.SmoothStep(0, 1, alpha);

            transform.position = (Vector3) Vector2.Lerp(start, point, alpha) + Vector3.forward * -2;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, alpha*1.1f);

            await Task.Yield();
        }

        transform.position = (Vector3) point+ Vector3.forward * -2;
        transform.rotation = targetRotation;

    }

    /// <summary>
    /// suit le chemin donné
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="speed"></param>
    /// <param name="smoothstep"></param>
    /// <returns></returns>
    public async Task FollowPath(Stack<Node> nodes,float speed, bool smoothstep = true)
    {
        while(nodes.Count > 0)
        {
            await MoveToPoint(nodes.Pop().transform.position, speed, smoothstep);
        }
    }

    /// <summary>
    /// trouve le chemin le plus court vers le noeud et fait bouger le bonhomme vers lui.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public async Task TravelToNodeThroughGraph(Node node, Astar _pathFinder)
    {
        _pathFinder.ResetNodes();
        Stack<Node> path = await _pathFinder.ComputePath(_currentNode, node);
        if (path.Count <= 1) return;

        await FollowPath(path, _moveSpeed);
        _currentNode = node;
    }
}

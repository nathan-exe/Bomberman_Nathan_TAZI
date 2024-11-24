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
    public TileAstarNode CurrentNode { get; private set; }
    public float _moveSpeed;

    public event Action OnMove;

    /// <summary>
    /// se déplace vers le point donné.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="speed"></param>
    /// <param name="smoothstep"></param>
    /// <returns></returns>
    public async Task MoveToPoint(TileAstarNode node,float speed,bool smoothstep = true) 
    {
        Quaternion startRotation = transform.rotation;
        Vector2 toPoint = (Vector2)transform.position - (Vector2)node.transform.position ;
        Quaternion targetRotation = Quaternion.Euler(0, 0,Mathf.Atan2(toPoint.y,toPoint.x)*Mathf.Rad2Deg+180);

        Vector2 start =transform.position;
        float startTime = Time.time;
        float duration = Vector2.Distance(start, (Vector2)node.transform.position) /speed;
        float endTime = Time.time+duration;

        CurrentNode = node;
        OnMove?.Invoke();
        while (Time.time < endTime)
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

    }

    /// <summary>
    /// téléporte le bonhomme vers la position et le snap sur la grille
    /// </summary>
    /// <param name="pose"></param>
    public void TeleportToPosition(Vector2 pose)
    {
        transform.position = (Vector3)pose.Round()-Vector3.forward*2f;
        CurrentNode = Graph.Instance.Nodes[pose.RoundToV2Int()];
    }

    private void Start()
    {
        TeleportToPosition(transform.position);
    }
}



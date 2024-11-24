using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Ce type de noeud represente une case de la tilemap, et permet de faire du
/// pathfinding dedans. le calcul du cout est basé sur la distance entre la case
/// et la cible, ainsi que sur g.
/// </summary>
[Serializable]
public class TileAstarNode : AstarNode
{
    public NodeContainer monoBehaviour;
    public GameObject gameObject => monoBehaviour.gameObject;
    public Transform transform => monoBehaviour.transform;
    public Vector2Int pose => gameObject.transform.position.RoundToV2Int();


    public const float gWheight = .5f;

    public override float compute_h(AstarNode target)
    {
        return Vector2.Distance(((TileAstarNode)target).monoBehaviour.transform.position, monoBehaviour.transform.position);
    }

    public override bool isActive()
    {
        return monoBehaviour.isActiveAndEnabled;
    }

    /// <summary>
    /// ici, f = h+g (comme dans le cour)
    /// </summary>
    /// <returns></returns>
    public override float ComputeCost()
    {
         return h + g * gWheight;
    }

}

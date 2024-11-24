using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFinder : MonoBehaviour
{
    public abstract Stack<AstarNode> ComputePath(AstarNode from, AstarNode to);
}

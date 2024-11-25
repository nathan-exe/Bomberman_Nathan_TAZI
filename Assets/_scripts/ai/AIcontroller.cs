using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;



public class AiController : MonoBehaviour
{
    //references
    Move _movement;
    BombBag _bombBag;
    [SerializeField] PathFinder _pathfinder;

    //mouvement
    Stack<AstarNode> _path = new();
    Task _currentMovementTask;


    /// <summary>
    /// change la destination vers laquelle se deplace le bonhomme et recalcule le chemin
    /// </summary>
    /// <param name="targetNode"></param>
    public void SetDestination(AstarNode targetNode)
    {
        _path = _pathfinder.ComputePath(_movement.CurrentNode, targetNode);
    }

    public void Move(Vector2Int offset)
    {
        _path.Clear();
        _=_movement.MoveToPoint(Graph.Instance.Nodes[ transform.position.RoundToV2Int() + offset],_movement._moveSpeed);
    }

    public void PlaceBomb()
    {
        _bombBag.TryToUseBomb();
    }

    private void Update()
    {
        //se déplace constamment sur le chemin.
        if ((_currentMovementTask == null||_currentMovementTask.IsCompleted) && _path.Count > 0)
        {
            _currentMovementTask = _movement.MoveToPoint((TileAstarNode)_path.Pop(), _movement._moveSpeed, true);
        }
    }

    private void Awake()
    {
        TryGetComponent<Move>(out _movement);
        TryGetComponent<BombBag>(out _bombBag);
    }

}

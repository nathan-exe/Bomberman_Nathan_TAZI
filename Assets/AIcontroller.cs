using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;



public class AiController : MonoBehaviour
{
    //references
    Move _movement;
    [SerializeField] Astar _pathfinder;

    //mouvement
    Stack<Node> _path = new();
    Task _currentMovementTask;

    //Events
    public event Action OnStep;

    /// <summary>
    /// change la destination vers laquelle se deplace le bonhomme et recalcule le chemin
    /// </summary>
    /// <param name="targetNode"></param>
    public void SetDestination(Node targetNode)
    {
        _path = _pathfinder.ComputePath(_movement.CurrentNode, targetNode);
    }


    private void Update()
    {
        //se d�place constamment sur le chemin.
        if ((_currentMovementTask == null||_currentMovementTask.IsCompleted) && _path.Count > 0)
        {
            OnStep?.Invoke();

            _currentMovementTask = _movement.MoveToPoint(_path.Pop(), _movement._moveSpeed, true);
        }
    }

    private void Awake()
    {
        TryGetComponent<Move>(out _movement);
    }

    private void Start()
    {
        _movement.TeleportToPosition(transform.position);
    }

}

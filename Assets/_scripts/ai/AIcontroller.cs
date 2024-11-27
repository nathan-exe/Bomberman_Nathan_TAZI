using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


/// <summary>
/// ce monobehaviour contient des méthodes publiques permettant 
/// de faire réaliser des actions au bot.
/// </summary>
public class AiController : MonoBehaviour
{
    //references
    Move _movement;
    BombBag _bombBag;
    [SerializeField] Astar _pathfinder;

    //mouvement
    Stack<AstarNode> _path = new();
    Task _currentMovementTask;

    public event Action OnIdle;

    /// <summary>
    /// change la destination vers laquelle se deplace le bonhomme et recalcule le chemin.
    /// </summary>
    /// <param name="targetNode"></param>
    public void SetDestination(AstarNode targetNode)
    {
        _path = _pathfinder.ComputePath(_movement.CurrentNode, targetNode);
    }

    /// <summary>
    /// pose une bombe par terre
    /// </summary>
    public void PlaceBomb()
    {
        _bombBag.TryToUseBomb();
    }

    private void Update()
    {
        //il se déplace constamment le long du chemin,jusqu'à ce qu'il soit vide.
        if ((_currentMovementTask == null || _currentMovementTask.IsCompleted))
        {
            if ( _path.Count > 0)
            {
                _currentMovementTask = _movement.MoveToPoint((TileAstarNode)_path.Pop(), _movement._moveSpeed, true);
            }
            else
            {
                OnIdle?.Invoke();
            }
        }
        
    }

    private void Awake()
    {
        TryGetComponent<Move>(out _movement);
        TryGetComponent<BombBag>(out _bombBag);
    }

}

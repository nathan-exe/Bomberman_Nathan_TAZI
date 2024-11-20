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
    BombBag _bombing;
    [SerializeField] Astar _pathfinder;
    [SerializeField] BreakableWall _ennemyWall;

    Node currentTarget;

    Stack<Node> _path = new();
    Task _currentMovementTask;

    private async void Update()
    {

        if ((_currentMovementTask == null||_currentMovementTask.IsCompleted) && _path.Count > 0)
        {
            //_path = _pathfinder.ComputePath(_movement._currentNode, FindBestTarget());
        foreach (Node node in _path) { print(node.name); }

            _path.Pop();
            _currentMovementTask = _movement.MoveToPoint(_path.Pop(), _movement._moveSpeed, true);
        }
        //print(_currentMovementTask);
        //print(FindBestTarget());
    }

    private void Awake()
    {
        TryGetComponent<Move>(out _movement);
        TryGetComponent<BombBag>(out _bombing);

    }

    private void Start()
    {
        _movement.TeleportToPosition(transform.position);
        //_path = _pathfinder.ComputePath(_movement._currentNode, FindBestTarget());

        //play();
    }

   /*
    /// <summary>
    /// trouve la prochaine cible vers laquelle il faudrait se déplacer
    /// </summary>
    /// <returns></returns>
    Node FindBestTarget()
    {
        Node nextTarget;
        if (!_bombing.hasBombs && _ennemyWall!=null) //si il n'a pas de bombe, il va en chercher une
        {
            nextTarget = FindNearestBombItem();
        } 
        else if(_ennemyWall !=null)//sinon, il va sur le bloc le plus proche du mur pour la poser.
        {
            nextTarget = FindNearestNodeAroundWall(((Vector2)_ennemyWall.transform.position).RoundToInt());
        }else
        {
            nextTarget=null;
        }

        return nextTarget;
    }*/


}

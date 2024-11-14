using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;



public class AiController : MonoBehaviour
{
    //references
    Move _movement;
    BombBag _bombing;
    [SerializeField] Astar _pathfinder;
    [SerializeField] BreakableWall _ennemyWall;

    Node currentTarget;

    private void Awake()
    {
        TryGetComponent<Move>(out _movement);
        TryGetComponent<BombBag>(out _bombing);
    }

    private void Start()
    {
        _movement.TeleportToPosition(transform.position);
        play();
    }

    async void play()
    {
        do
        {
            Node nextTarget = FindBestTarget();

            if (currentTarget == nextTarget) await Task.Yield();
            currentTarget = nextTarget;

            bool targetWasReached = await _movement.TravelToNodeThroughGraph(currentTarget, _pathfinder, () => FindBestTarget() != currentTarget); //commence à avancer sur la cible
            if (targetWasReached)
            {
                if (!_bombing.CanPickUpBomb) _bombing.useBomb(); //quand il a atteint le mur
            }
        } while (isActiveAndEnabled && Application.isPlaying);
    }
    Node FindBestTarget()
    {
        Node nextTarget;
        if (_bombing.CanPickUpBomb) //si il n'a pas de bombe, il va en chercher une
        {
            nextTarget = FindNearestBombItem();
        } 
        else //sinon, il va sur le bloc le plus proche du mur pour la poser.
        {
            nextTarget = FindNearestNodeAroundWall(((Vector2)_ennemyWall.transform.position).RoundToInt());
        }
        return nextTarget;
    }

    private Node FindNearestNodeAroundWall(Vector2Int wall)
    {
        float minDistanceSquared = float.PositiveInfinity;
        Vector2Int closest = wall+Vector2Int.up;

        foreach ( Vector2Int offset in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left })
        {
            Vector2Int pose = wall + offset;
            if (Graph.Instance.Nodes.ContainsKey(pose) && Graph.Instance.Nodes[pose].isActiveAndEnabled)
            {
                float dsq = (pose - (Vector2)transform.position).sqrMagnitude;
                if (dsq < minDistanceSquared)
                {
                    minDistanceSquared = dsq;
                    closest = pose;
                }
            }
        }
        
        return Graph.Instance.Nodes[closest];
    }

    private Node FindNearestBombItem()
    {
        float minDistanceSquared = float.PositiveInfinity; 
        Transform closest = null;
        foreach ( BombItem bomb in BombItem.freeBombs)
        {
            float dsq = (bomb.transform.position - transform.position).sqrMagnitude;
            if (dsq < minDistanceSquared)
            {
                minDistanceSquared = dsq;
                closest = bomb.transform;
            } 
        }

        return closest == null ? null : Graph.Instance.Nodes[((Vector2)closest.position).RoundToInt()];
    }

}

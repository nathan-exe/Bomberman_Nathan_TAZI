using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class State_FleeingBomb : StateBase
{
    private List<Bomb> _nearbyBombs = new();
    public State_FleeingBomb(StateMachine sm) : base(sm)
    {
    }
    public override bool CanBeEnteredFromContext(GameContext ctx)
    {
        return ctx.DangerousBombsAroundAgent>0 && ctx.AgentHp > - ctx.DangerousBombsAroundAgent ;//on met "-" pour compenser les PVs qu'il a perdu sur la simulation d'avant en restant à coté de la bombe
    }
    public override GameContext SimulateOutcomeContext(GameContext ctx)
    {
        ctx.AgentHp += ctx.DangerousBombsAroundAgent ;//on met "+=" pour compenser les PVs qu'il a perdu sur la simulation d'avant en restant à coté de la bombe
        ctx.PlayerHp -= ctx.DangerousBombsAroundPlayer ;
        return ctx;
    }


    public override void OnEntered()
    {
        machine.Sensor.OnAgentMoved += ReactToNearbyBombs;
        machine.Sensor.OnBombPlacedByAgent += ReactToNearbyBombs;
        machine.Sensor.OnBombPlacedByPlayer += ReactToNearbyBombs;
    }

    public override void OnExited()
    {
        machine.Sensor.OnAgentMoved -= ReactToNearbyBombs;
        machine.Sensor.OnBombPlacedByAgent -= ReactToNearbyBombs;
        machine.Sensor.OnBombPlacedByPlayer -= ReactToNearbyBombs;
    }

    public override void Update()
    {
    }

    public void ReactToNearbyBombs()
    {
        //machine.Controller.SetDestination(Graph.Instance.Nodes[machine.transform.position.RoundToV2Int()+Vector2Int.up]);
        //si il est dans le rayon d'action d'une bombe
        machine.Sensor.FindTickingBombsAroundPoint_NoAlloc(machine.transform.position.Round(),ref _nearbyBombs);
        if (_nearbyBombs.Count > 0) 
        {
            //il se déplace vers une case libre et safe aléatoire
            List<Vector2Int> freeTiles = machine.Sensor.FindFreeNodesAroundPoint(machine.transform.position);
            foreach (Vector2Int tile in freeTiles) Debug.DrawRay(tile.ToVector3(), Vector3.up*0.3f, Color.white,1);

            List<Vector2Int> safeTiles = new();
            foreach (Vector2Int tile in freeTiles) if (machine.Sensor.IsTileSafe(tile)) safeTiles.Add(tile);
            
            foreach (Vector2Int tile in safeTiles) Debug.DrawRay(tile.ToVector3(), Vector3.up * 0.1f, Color.red, 1);
            EditorApplication.isPaused = true;
            if (safeTiles.Count > 0) machine.Controller.SetDestination(Graph.Instance.Nodes[safeTiles[Random.Range(0, safeTiles.Count)]]);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_ChasingPlayer : StateBase
{
    AstarNode _currentTarget;
    public State_ChasingPlayer(StateMachine sm) : base(sm)
    {
    }

    public override bool CanBeEnteredFromContext(GameContext ctx)
    {
        return ctx.AgentHp > 0 && ctx.AgentBombCount > 0;
    }

    public override GameContext SimulateOutcomeContext(GameContext ctx)
    {
        ctx.AgentHp -= ctx.DangerousBombsAroundAgent + ctx.PlayerBombCount*0.5f;
        ctx.PlayerHp -= ctx.DangerousBombsAroundPlayer + ctx.AgentBombCount*0.5f;
        ctx.AgentBombCount -=1;
        return ctx;
    }

    public override void OnEntered()
    {
        GoToPlayer();
        machine.Sensor.OnPlayerMoved += GoToPlayer;
        machine.Sensor.OnAgentMoved += TryToPlaceBombs;
        machine.Controller.OnIdle += GoToPlayer;

    }

    public override void OnExited()
    {
        machine.Sensor.OnPlayerMoved -= GoToPlayer;
        machine.Sensor.OnAgentMoved -= TryToPlaceBombs;
        machine.Controller.OnIdle -= GoToPlayer;

    }

    public override void Update()
    {
    }

    /// <summary>
    /// trouve le noeud sur lequel se trouve le joueur et recalcule le chemin si il a changé.
    /// </summary>
    void GoToPlayer()
    {
        AstarNode PlayerNode = machine.Sensor.FindNearestNodeAroundPlayer();
        if (PlayerNode != null && (PlayerNode != _currentTarget || _currentTarget == null))
        {
            _currentTarget = PlayerNode;

            machine.Controller.SetDestination(_currentTarget);
        }
    }

    /// <summary>
    /// a 2 chances sur 3 de placer une bombe 
    /// </summary>
    void TryToPlaceBombs()
    {
        if((machine.transform.position-machine.Sensor.PlayerPosition.ToVector3()).sqrMagnitude<= 9)
        {
            //pour aller plus loin : faire que la chance dépende du nombre de murs autour,
            //pour l'encourager à bloquer les passages étroits
            if (Random.value > 0.3f) 
            {
                machine.Controller.PlaceBomb();
            }
        }
    }

}

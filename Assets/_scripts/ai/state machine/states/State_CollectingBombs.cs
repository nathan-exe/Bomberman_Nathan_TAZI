using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class State_CollectingBombs : StateBase
{
    public State_CollectingBombs(StateMachine sm) : base(sm) { }

    public override bool CanBeEnteredFromContext(GameContext ctx)
    {
        return ctx.AgentHp > 0;
    }
    public override GameContext SimulateOutcomeContext(GameContext ctx)
    {
        ctx.AgentHp -= ctx.DangerousBombsAroundAgent + ctx.PlayerBombCount*0.5f; //chaque bombe dans l'inventaire du joueur aurait une chance sur deux de faire des dégats au bot
        ctx.PlayerHp -= ctx.DangerousBombsAroundPlayer;

        ctx.AgentBombCount++;
        return ctx;
    }

    public override void OnEntered()
    {
        FindPathToNearestBomb();
        machine.Sensor.OnBombPickedUpByPlayer += FindPathToNearestBomb;
        machine.Sensor.OnBombPickedUpByAgent += FindPathToNearestBomb;
        machine.Controller.OnIdle += FindPathToNearestBomb;
    }

    public override void OnExited()
    {
        machine.Sensor.OnBombPickedUpByPlayer -= FindPathToNearestBomb;
        machine.Sensor.OnBombPickedUpByAgent -= FindPathToNearestBomb;
        machine.Controller.OnIdle -= FindPathToNearestBomb;
    }



    public override void Update()
    {
    }

    /// <summary>
    /// trouve la bombe la plus proche et recalcule le chemin vers elle
    /// </summary>
    void FindPathToNearestBomb()
    {        
        machine.Controller.SetDestination(machine.Sensor.FindNearestBombNode());
    }
}

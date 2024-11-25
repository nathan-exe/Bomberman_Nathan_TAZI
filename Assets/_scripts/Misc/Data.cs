using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameWinState { win,lose,playing}

[Serializable]
public struct GameContext//recalculer le pathfinding des que ça change
{
    public int AgentHp;
    public int PlayerHp;
    public int AgentBombCount;
    public float DistanceToPlayer;
}



[Serializable]
public struct StateConditions
{
    public StateConditions(int bombTreshold, int playerHPTreshold, int HPTreshold)
    {
        this.AgentBombTreshold = bombTreshold;
        this.PlayerHpTreshold = playerHPTreshold;
        this.AgentHpTreshold = HPTreshold;
    }

    public int AgentBombTreshold;
    public int PlayerHpTreshold;
    public int AgentHpTreshold;
}

[Serializable]
public struct StateCostWeights
{
    public float AgentHpweight;
    public float playerHpweight;
    public float AgentbombGainWeight;
}

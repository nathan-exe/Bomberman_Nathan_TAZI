using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameWinState { win,lose,playing}

[Serializable]
public struct GameContext//recalculer le pathfinding des que ça change
{
    public float AgentHp;
    public float PlayerHp;
    public int AgentBombCount;
    public int PlayerBombCount;
    public int DangerousBombsAroundAgent;
    public int DangerousBombsAroundPlayer;

    public GameContext(float agentHp, float playerHp, int agentBombCount, int playerBombCount, int dangerousBombsAroundAgent, int dangerousBombsAroundPlayer)
    {
        AgentHp = agentHp;
        PlayerHp = playerHp;
        AgentBombCount = agentBombCount;
        PlayerBombCount = playerBombCount;
        DangerousBombsAroundAgent = dangerousBombsAroundAgent;
        DangerousBombsAroundPlayer = dangerousBombsAroundPlayer;
    }

    public override string ToString()
    {
        return $"AgentHp = {AgentHp};\r\nPlayerHp = {PlayerHp};\r\nAgentBombCount = {AgentBombCount};\r\nPlayerBombCount = {PlayerBombCount};\r\nDangerousBombsAroundAgent = {DangerousBombsAroundAgent};\r\nDangerousBombsAroundPlayer = {DangerousBombsAroundPlayer};";
    }
}



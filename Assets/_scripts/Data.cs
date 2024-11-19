using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct IAGameState//recalculer le pathfinding des que ça change
{
    public int HP;
    public int playerHP;
    public int bombCount;
}

public struct StateOutcome
{
    public StateOutcome(int potentialHPgain, int potentialPlayerHPgain, int potentialBombGain)
    {
        this.potentialHPgain = potentialHPgain;
        this.potentialPlayerHPgain = potentialPlayerHPgain;
        this.potentialBombGain = potentialBombGain;
    }

    public int potentialHPgain;
    public int potentialPlayerHPgain;
    public int potentialBombGain;
}
public struct StateConditions
{
    public StateConditions(int bombTreshold, int playerHPTreshold, int HPTreshold)
    {
        this.bombTreshold = bombTreshold;
        this.playerHPTreshold = playerHPTreshold;
        this.HPTreshold = HPTreshold;
    }

    int bombTreshold;
    int playerHPTreshold;
    int HPTreshold;
}

struct StateWeights
{
    public float HPweight;
    public float playerHPweight;
    public float bombGainWeight;
}

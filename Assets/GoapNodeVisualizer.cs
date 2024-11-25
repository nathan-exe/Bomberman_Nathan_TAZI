using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoapNodeVisualizer : MonoBehaviour
{
    [SerializeField] TMP_Text AgentHp, PlayerHp, AgentBombCount, DistanceToPlayer;
    [SerializeField] Image _image;
    public void UpdateUI(GameContext newContext,bool canBeEnteredFromPreviousState,bool isCurrentState)
    {
        AgentHp.text = newContext.AgentHp.ToString();
        PlayerHp.text = newContext.PlayerHp.ToString();
        AgentBombCount.text = newContext.AgentBombCount.ToString();
        DistanceToPlayer.text = newContext.DistanceToPlayer.ToString();

        if (isCurrentState)
        {
            _image.color = Color.cyan;
        }
        else if (canBeEnteredFromPreviousState) _image.color = Color.white;
        else _image.color = Color.black;
    }
}

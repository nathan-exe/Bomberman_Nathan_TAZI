using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public TMP_Text text;

    //il est une heure du matin me tue pas pour le singleton nul stp Romain j'en peux plus
    public static GameOver Instance { private set; get; }
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    //KISS
    public void triggerPlayerWin()
    {
        gameObject.SetActive(true);
        text.text = "Player Wins !";
    }
    //KISS
    public void triggerBotWin()
    {
        gameObject.SetActive(true);
        text.text = "Bot Wins !";
    }

}

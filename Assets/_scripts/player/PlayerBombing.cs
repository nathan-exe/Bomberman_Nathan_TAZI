using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBag : MonoBehaviour
{
    public Stack<BombItem> BombStack = new();
    public bool hasBombs => BombStack.Count>0;

    public event Action OnBombPickedUp;
    public event Action OnBombPlaced;


    public void pickUpBomb(BombItem bomb)
    {
        BombStack.Push(bomb) ;
        OnBombPickedUp?.Invoke();
    }

    public void TryToUseBomb()
    {
        if(hasBombs)
        {
            useBomb();
        }
    }

    public void useBomb()
    {
        PoolReferences.Instance.BombPool.PullObjectFromPool((Vector3)((Vector2)transform.position).Round()-Vector3.forward);
        BombStack.Pop().Respawn();
        StartCoroutine(boule());
    }

    IEnumerator boule() //je vais peter un cable romain
    {
        yield return new WaitForFixedUpdate();
        OnBombPlaced?.Invoke();
    }
}

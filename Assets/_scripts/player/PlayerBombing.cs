using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBag : MonoBehaviour
{
    public Stack<BombItem> BombStack = new();
    public bool hasBombs => BombStack.Count>0;

    public event Action OnBombPickedUp;


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
        //pull bomb from pool
        PoolReferences.Instance.BombPool.PullObjectFromPool((Vector3)((Vector2)transform.position).Round()-Vector3.forward); //@extdrcfygvubhinjo,kp;lzr^qesgr:hdfmcbvxjopihulgykfhvihbjl,
        BombStack.Pop().Respawn();
    }
}

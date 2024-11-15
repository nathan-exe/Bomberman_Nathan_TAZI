using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBag : MonoBehaviour
{

    BombItem bombItem = null;

    public bool CanPickUpBomb => bombItem == null;

    public void pickUpBomb(BombItem bomb)
    {
        bombItem = bomb;
    }

    public void TryToUseBomb()
    {
        if(bombItem != null)
        {
            useBomb();
        }
    }

    public void useBomb()
    {
        //pull bomb from pool
        PoolReferences.Instance.BombPool.PullObjectFromPool((Vector3)((Vector2)transform.position).Round()-Vector3.forward); //@extdrcfygvubhinjo,kp;lzr^qesgr:hdfmcbvxjopihulgykfhvihbjl,
        bombItem.Respawn();
        bombItem = null;
    }
}

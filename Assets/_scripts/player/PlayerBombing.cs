using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombing : MonoBehaviour
{

    BombItem bombItem = null;

    public bool CanPickUpBomb => bombItem == null;

    public void pickUpBomb(BombItem bomb)
    {
        bombItem = bomb;
    }

    public void useBomb()
    {
        //pull bomb from pool
        FindObjectOfType<Pool>().PullObjectFromPool(); //@extdrcfygvubhinjo,kp;lzr^qesgr:hdfmcbvxjopihulgykfhvihbjl,
        bombItem = null;
    }
}

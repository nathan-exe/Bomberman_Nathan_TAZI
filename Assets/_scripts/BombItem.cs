using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombItem : Item
{
    protected override void OnItemPickedUp(GameObject player)
    {
        if(player.TryGetComponent<PlayerBombing>(out PlayerBombing p))
        {
            if (p.CanPickUpBomb)
            {
                p.pickUpBomb(this);
                gameObject.SetActive(false);
            }
        }
    }

    public void respawn()
    {
        transform.position =  (Vector2) Graph.Instance.Nodes.Keys.ToArray<Vector2Int>()[Random.Range(0, Graph.Instance.Nodes.Keys.Count)];
    }
}

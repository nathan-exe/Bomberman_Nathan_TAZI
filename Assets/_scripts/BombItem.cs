using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombItem : Item
{
    public static List<BombItem> freeBombs = new();

    private void OnEnable()
    {
        freeBombs.Add(this);
    }

    private void OnDisable()
    {
        freeBombs.Remove(this);
    }

    protected override void OnItemPickedUp(GameObject player)
    {
        if(player.TryGetComponent<BombBag>(out BombBag p))
        {
            if (p.CanPickUpBomb)
            {
                p.pickUpBomb(this);
                gameObject.SetActive(false);
            }
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        transform.position =  (Vector2) Graph.Instance.FreeNodes[Random.Range(0, Graph.Instance.FreeNodes.Count)].pose;
        transform.position -= Vector3.forward;
    }
}

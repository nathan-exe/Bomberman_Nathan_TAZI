using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// por tous les objets pouvan etre ramassés en passant dessus
/// </summary>
public abstract class Item : MonoBehaviour
{
    private const string _playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject);
        if (collision.gameObject.tag == _playerTag)
        {
            OnItemPickedUp(collision.gameObject);
        }
    }

    protected abstract void OnItemPickedUp(GameObject player);
}

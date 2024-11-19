using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject _explosionVisual;

    PooledObject _asPooledObject;
    const string _explosionMessage = "OnBlownUp";
    //messages appel�s par la pool

    public void OnInstantiatedByPool()
    {
        TryGetComponent<PooledObject>(out _asPooledObject);
    }

    public void OnPulledFromPool()
    {
        _explosionVisual.SetActive(false);
        StartCoroutine(ExplodeWithDelay());
    }

    IEnumerator ExplodeWithDelay()
    {
        yield return new WaitForSeconds(3);

        SendHitMessagesToSurroundingObjects();

        _explosionVisual.SetActive(true);
        yield return new WaitForSeconds(.2f);
        _explosionVisual.SetActive(false);

        GetComponent<PooledObject>().GoBackIntoPool();
    }

    /// <summary>
    /// dit aux objets autour de la bombe de r�agir � l'explosion si ils sont gentils
    /// </summary>
    void SendHitMessagesToSurroundingObjects()
    {
        Collider2D[] col = new Collider2D[1];
        foreach (Vector2Int offset in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left })
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, offset, 3, ~LayerMask.GetMask(Graph.NodeLayer));
            if (hit) hit.collider.gameObject.SendMessage(_explosionMessage, SendMessageOptions.DontRequireReceiver);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// ce script est attach� � la bombe apr�s qu'elle ait �t� pos�e par terre par le joueur ou le bot,
/// elle est dangereuse et explose au bout d'un certain temps
/// </summary>
public class TickingBomb : MonoBehaviour
{
    [SerializeField] GameObject _explosionVisual;

    PooledObject _asPooledObject;
    const string _explosionMessage = "OnBlownUp";
    //messages appel�s par la pool

    public void OnInstantiatedByPool()
    {
        TryGetComponent<PooledObject>(out _asPooledObject);
    }

    //equivalent de void start()
    public void OnPulledFromPool()
    {
        _explosionVisual.SetActive(false);

        Graph.Instance.DisableNode(Graph.Instance.Nodes[transform.position.RoundToV2Int()]);
        StartCoroutine(ExplodeWithDelay());
    }

    /// <summary>
    /// attend un peu puis explose
    /// </summary>
    /// <returns></returns>
    IEnumerator ExplodeWithDelay()
    {
        yield return new WaitForSeconds(1.5f);

        SendHitMessagesToSurroundingObjects();

        _explosionVisual.SetActive(true);
        yield return new WaitForSeconds(.2f);
        _explosionVisual.SetActive(false);

        Graph.Instance.ActivateNodeAtPosition(transform.position.RoundToV2Int());
        GetComponent<PooledObject>().GoBackIntoPool();
    }

    /// <summary>
    /// dit aux objets autour de la bombe de r�agir � l'explosion si ils sont gentils
    /// </summary>
    void SendHitMessagesToSurroundingObjects()
    {
        Collider2D[] col = new Collider2D[1];
        foreach (Vector2Int offset in VectorExtensions.AllFourDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position+offset.ToVector3() *0.51f, offset, 3, ~LayerMask.GetMask(Graph.NodeLayer));
            if (hit) hit.collider.gameObject.SendMessage(_explosionMessage, SendMessageOptions.DontRequireReceiver);
        }
    }
}
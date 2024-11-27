using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// la bombe après qu'elle ait été posée par terre par le joueur ou le bot,
/// elle est dangereuse et explose au bout d'un certain temps
/// </summary>
public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject _explosionVisual;

    PooledObject _asPooledObject;
    const string _explosionMessage = "OnBlownUp";
    //messages appelés par la pool

    public void OnInstantiatedByPool()
    {
        TryGetComponent<PooledObject>(out _asPooledObject);
    }

    public void OnPulledFromPool()
    {
        _explosionVisual.SetActive(false);
        Graph.Instance.RemoveNodeFromGraph(Graph.Instance.Nodes[transform.position.RoundToV2Int()]);

        StartCoroutine(ExplodeWithDelay());
    }

    IEnumerator ExplodeWithDelay()
    {
        yield return new WaitForSeconds(1.5f);

        SendHitMessagesToSurroundingObjects();

        _explosionVisual.SetActive(true);
        yield return new WaitForSeconds(.2f);
        _explosionVisual.SetActive(false);

        Graph.Instance.AddNodeToGraph(transform.position.RoundToV2Int());
        GetComponent<PooledObject>().GoBackIntoPool();
    }

    /// <summary>
    /// dit aux objets autour de la bombe de réagir à l'explosion si ils sont gentils
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

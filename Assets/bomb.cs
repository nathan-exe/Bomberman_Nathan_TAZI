using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
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


    void SendHitMessagesToSurroundingObjects()
    {
        Collider2D[] col = new Collider2D[1];

        Vector2 pose = (Vector2)transform.position + Vector2.up;
        if (Physics2D.OverlapPointNonAlloc(pose, col) > 0) col[0].gameObject.SendMessage(_explosionMessage);

        pose = (Vector2)transform.position + Vector2.down;
        if (Physics2D.OverlapPointNonAlloc(pose, col) > 0) col[0].gameObject.SendMessage(_explosionMessage);

        pose = (Vector2)transform.position + Vector2.right;
        if (Physics2D.OverlapPointNonAlloc(pose, col) > 0) col[0].gameObject.SendMessage(_explosionMessage);

        pose = (Vector2)transform.position + Vector2.left;
        if (Physics2D.OverlapPointNonAlloc(pose, col) > 0) col[0].gameObject.SendMessage(_explosionMessage);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public void OnBlownUp()
    {
        print("boom");
        Destroy(gameObject);
    }
}

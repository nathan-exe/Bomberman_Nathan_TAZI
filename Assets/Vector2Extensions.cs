using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions 
{
    public static Vector2Int RoundToInt(this Vector2 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static Vector2 Round(this Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }
}

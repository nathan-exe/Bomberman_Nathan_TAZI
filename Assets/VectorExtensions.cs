using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class VectorExtensions 
{
    public static readonly Vector2Int[] AllFourDirections = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left }; //mettre ça ici me permet de ne pas instancier de nouveaux arrays à chaque fois que je veux faire un foreach.
    public static Vector2Int RoundToV2Int(this Vector2 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }
    public static Vector2Int RoundToV2Int(this Vector3 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static Vector2 Round(this Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static Vector3 Round(this Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y),Mathf.Round(v.z));
    }

    public static Vector3 ToVector3(this Vector2Int v)
    {
        return (Vector3)(Vector2)v;
    }

}

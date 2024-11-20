using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIsensor : MonoBehaviour
{

    [Header("References")]
    [SerializeField] HealthComponent _playerHPcomponent;
    [SerializeField] HealthComponent _AiHPcomponent;
    [SerializeField] BombBag _AibombBag;
    
    public int PlayerHP => _playerHPcomponent.HP;
    public int AgentHP => _playerHPcomponent.HP;
    public int AgentBombCount => _AibombBag.BombStack.Count;

    /// <summary>
    /// trouve la bombe ramassable la plus proche
    /// </summary>
    /// <returns></returns>
    public Node FindNearestBombItem()
    {
        float minDistanceSquared = float.PositiveInfinity;
        Transform closest = null;
        foreach (BombItem bomb in BombItem.freeBombs)
        {
            float dsq = (bomb.transform.position - transform.position).sqrMagnitude;
            if (dsq < minDistanceSquared)
            {
                minDistanceSquared = dsq;
                closest = bomb.transform;
            }
        }

        return closest == null ? null : Graph.Instance.Nodes[((Vector2)closest.position).RoundToInt()];
    }
}

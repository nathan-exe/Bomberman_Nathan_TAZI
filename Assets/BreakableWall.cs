using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakableWall : MonoBehaviour
{
    const int maxHP = 10;
    int _hp = maxHP;

    [SerializeField] Slider _lifebar;

    public void OnBlownUp()
    {
        _hp--;
        _lifebar.value = _hp;
        if ( _hp <= 0 ) Destroy(gameObject);
    }
}

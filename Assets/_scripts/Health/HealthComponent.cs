using System;
using UnityEngine;

/// <summary>
/// ce monobehaviour sert à gérer les points de vie d'une unité.
/// </summary>
public class HealthComponent : MonoBehaviour
{

    [SerializeField] bool _destroyGameObjectOnDeath;
    public int MaxHP = 100;

    //HP getters et setters
    private int _HP;
    public int HP
    {
        get { return _HP; }
        private set
        {
            _HP = Mathf.Clamp(value, 0, MaxHP);
            OnHealthUpdated?.Invoke(_HP);
            if(HP==0)OnDeath?.Invoke();
        }
    }

    //notifiers
    public event Action<int> OnHealthUpdated;
    public event Action OnDeath;
    public event Action OnDamageTaken;

    /// <summary>
    /// fait des dégats au joueur.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        OnDamageTaken?.Invoke();
        HP = HP - Mathf.Abs(damage);
    }

    /// <summary>
    /// soigne le joueur.
    /// </summary>
    /// <param name="heal"></param>
    public void Heal(int heal)
    {
        HP = HP + Mathf.Abs(heal);
    }


    private void Awake()
    {
        //Destroy object on death
        if (_destroyGameObjectOnDeath) OnDeath += () =>
        {
            if (TryGetComponent<PooledObject>(out PooledObject asPooledObject)) asPooledObject.GoBackIntoPool();
            else Destroy(gameObject);
        };
        HP = MaxHP;
    }


    public void OnBlownUp()
    {
        TakeDamage(1);
    }

}

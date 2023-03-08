using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int MaximumHealth = 100;
    private int _health;
    private bool _isDead => _health == 0;
    private bool isInvunerable;
    public event Action OnTakeDamage;
    public event Action OnDie;

    private void Start()
    {
        _health = MaximumHealth;
    }

    public void Invunerable(bool set)
    {
        isInvunerable = set;
    }

    public void DealDamage(int damage)
    {
        if (isInvunerable) return;
        _health = Mathf.Max(_health - damage, 0);

        OnTakeDamage?.Invoke();

        if (_isDead) { OnDie?.Invoke(); }

        Debug.Log(_health + " " + _isDead);
    }
}

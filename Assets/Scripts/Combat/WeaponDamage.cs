using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;
    private int _damage;
    private float _knockback;
    public List<Collider> AlreadyCollided = new List<Collider>();

    private void OnEnable()
    {
        AlreadyCollided.Clear();
    }

    public void SetDamage(int newDamage, float knockback)
    {
        this._damage = newDamage;
        this._knockback = knockback;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider) { return; }
        if (AlreadyCollided.Contains(other)) { return; }
        AlreadyCollided.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(_damage);
        }

        if (other.TryGetComponent<ForceReciver>(out ForceReciver forceReciver))
        {
            forceReciver.AddForce((other.transform.position - playerCollider.transform.position).normalized * _knockback);
        }
    }
}

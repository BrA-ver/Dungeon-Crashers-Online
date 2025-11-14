using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Health health;
    [SerializeField] Character attacker;

    [Header("Collisions")]
    [SerializeField] DamageFlash[] flashs;

    public UnityEvent OnTookDamage;

    public void TakeDamage(int damage, Character attacker)
    {
        foreach (DamageFlash flash in flashs)
        {
            flash.Flash();
        }
        OnTookDamage?.Invoke();
        this.attacker = attacker;
        health.TakeDamage(damage, attacker);
    }
}

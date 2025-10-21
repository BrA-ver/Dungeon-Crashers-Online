using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] DamageFlash[] flashs;

    public UnityEvent OnTookDamage;

    public void TakeDamage(int damage)
    {
        foreach (DamageFlash flash in flashs)
        {
            flash.Flash();
        }
        OnTookDamage?.Invoke();
    }
}

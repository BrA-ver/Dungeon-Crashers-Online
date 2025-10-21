using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] protected int damage = 2;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hitbox hitbox))
        {
            Debug.Log("Hit");
            hitbox.TakeDamage(damage);
        }
    }
}

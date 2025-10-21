using UnityEngine;

public class Bullet : Hurtbox
{
    [SerializeField] LayerMask ignoreLayers;

    protected override void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & ignoreLayers.value) != 0)
            return;
        Debug.Log(other.name);

        if (other.TryGetComponent(out Hitbox hitbox))
        {
            Debug.Log("Hit");
            hitbox.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

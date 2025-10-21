using UnityEngine;

public class WizardCombatManager : PlayerCombatManager
{
    [SerializeField] Rigidbody bullet;
    [SerializeField] float shootSpeed;
    [SerializeField] Transform bulletSpawn;

    public override void Attack()
    {
        //base.Attack();
        isAttacking = true;
        player.AnimationHandler.PlayTargetAnimation("Shoot");
    }

    public void ShootBulllet()
    {
        Rigidbody newBullet = Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
        newBullet.transform.forward = transform.forward;
        newBullet.linearVelocity = transform.forward * shootSpeed;
    }
}

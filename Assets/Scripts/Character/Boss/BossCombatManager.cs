using UnityEngine;

public class BossCombatManager : EnemyCombatManager
{
    public override void AttackDash()
    {
        base.AttackDash();
        character.Movement.Dash(currentAttack.dashSpeed, currentAttack.duration);
    }
}

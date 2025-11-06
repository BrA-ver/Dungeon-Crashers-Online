using UnityEngine;

public class Boss : Enemy
{
    protected BossMovement bossMovement;

    protected override void Awake()
    {
        base.Awake();

        bossMovement = movement as BossMovement;
    }
}

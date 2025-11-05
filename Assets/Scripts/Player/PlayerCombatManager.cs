using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    protected Player player;
    protected bool isAttacking;

    [Header("Attacks")]
    [SerializeField] Attack[] attackCombo;
    int comboIndex;

    [SerializeField] float comboReset = 1f;
    float resetCounter;

    protected override void Awake()
    {
        base.Awake();
        player = character as Player;
    }


    //protected virtual void Awake()
    //{
    //    player = GetComponent<Player>();
    //}

    protected virtual void Update()
    {
        if (!isAttacking)
        {
            if (resetCounter > 0f)
            {
                resetCounter -= Time.deltaTime;
                if (resetCounter <= 0f)
                {
                    resetCounter = 0f;
                    comboIndex = 0;
                }
            }
        }
    }

    public override void Attack()
    {
        isAttacking = true;
        Attack currentAttack = attackCombo[comboIndex];
        attackAnim = currentAttack.animName;
        player.Movement.Dash(currentAttack.dashSpeed, currentAttack.duration);
        base.Attack();
    }
    public override void FinishAttack()
    {
        isAttacking = false;
        player.isPerformingAction = false;
        comboIndex++;
        if (comboIndex >= attackCombo.Length)
        {
            comboIndex = 0;
        }

        resetCounter = comboReset;
    }
}

[System.Serializable]
public class Attack
{
    public string animName;
    public float dashSpeed;
    public float duration;
}
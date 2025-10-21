using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    protected Player player;
    protected bool isAttacking;

    [Header("Attacks")]
    [SerializeField] Attack[] attackCombo;
    int comboIndex;

    [SerializeField] float comboReset = 1f;
    float resetCounter;


    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }

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

    public virtual void Attack()
    {
        isAttacking = true;
        Attack currentAttack = attackCombo[comboIndex];
        player.AnimationHandler.PlayTargetAnimation(currentAttack.animName);
        player.PlayerMovement.Dash(currentAttack.dashSpeed, currentAttack.duration);
    }
    public virtual void FinishAttack()
    {
        if (isAttacking)
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
}

[System.Serializable]
public class Attack
{
    public string animName;
    public float dashSpeed;
    public float duration;
}
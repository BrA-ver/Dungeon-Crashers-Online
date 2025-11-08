using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    protected Player player;
    protected bool isAttacking;

    bool canRotate;

    [Header("Attacks")]
    [SerializeField] Attack[] attackCombo;
    Attack currentAttack;
    int comboIndex;

    [SerializeField] float comboReset = 1f;
    float resetCounter;

    [SerializeField] float rotationSpeed = 12f;

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
        else
        {
            if (canRotate)
            {
                Vector2 moveInput = InputHandler.instance.MoveInput;
                Vector3 moveDir = PlayerCamera.instance.transform.forward * moveInput.y;
                moveDir += PlayerCamera.instance.transform.right * moveInput.x;
                moveDir.y = 0f;
                moveDir.Normalize();

                HandleRotation(moveDir);
            }
        }
    }

    public void HandleRotation(Vector3 lookDir)
    {
        if (lookDir.magnitude < 0.1f) return;

        Quaternion lookRotation = Quaternion.LookRotation(lookDir);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }

    public override void Attack()
    {
        isAttacking = true;
        currentAttack = attackCombo[comboIndex];
        attackAnim = currentAttack.animName;
        
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

    public void AttackDash()
    {
        player.Movement.Dash(currentAttack.dashSpeed, currentAttack.duration);
    }

    public void StartRotation()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }
}


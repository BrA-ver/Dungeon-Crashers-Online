using UnityEngine;

public class Borax : Boss
{
    [SerializeField] bool sprint;

    [SerializeField] public Animator animator;

    [SerializeField, Range(1, 10)] int maxAttacks = 5;
    int attackCount = 0;

    [Header("Fireball")]
    [SerializeField] float fireballCooldown = 4f;
    float fireballCounter = 0f;
    bool canDoFireball;

    StateMachine stateMachine;

    [Header("Stun")]
    public float stunTime = 15f;
    public int poise = 7;
    public bool isStunned = false;
    public int hitCount;



    protected override void Awake()
    {
        base.Awake();
        stateMachine = GetComponent<StateMachine>();
    }

    protected override void Start()
    {
        base.Start();
        attackCount = maxAttacks;

        stateMachine.SwitchState(new BoraxIdleState(stateMachine, this));
        GetPlayerAsTarget();
        UIManager.instance.ShowBossHealth(this);
    }

    protected override void Update()
    {
        //if (!IsOwner)
        //    return;

        //fireballCounter -= Time.deltaTime;
        //if (fireballCounter <= 0f)
        //{
        //    canDoFireball = true;
        //}

        //if (Vector3.Distance(target.transform.position, transform.position) <= stoppingDistance)
        //{
        //    if (!movement.dashing)
        //        StopMoving();
        //    else
        //    {
        //        movement.HandleDash();
        //    }

        //    if (!isPerformingAction)
        //    {
        //        if (attackCount >= maxAttacks)
        //        {
        //            // Scream
        //            combatManager.Taunt();
        //            attackCount = 0;
        //        }
        //        else
        //        {
        //            if (canDoFireball)
        //            {
        //                // DO Fireball
        //                animator.CrossFade("Attack_Fireball", 0.1f);
        //                isPerformingAction = true;

        //                fireballCounter = fireballCooldown;
        //                attackCount++;
        //                canDoFireball = false;
        //            }
        //            else
        //            {

        //                combatManager.Attack();
        //                attackCount++;
        //            }
        //        }
        //    }
        //    AnimateMovement(sprint);
        //    return;
        //}

        //float speed = sprint ? bossMovement.SprintSpeed: bossMovement.Speed;
        //HandleMovement(speed);

        //AnimateMovement(sprint);
    }

    public virtual void HandleMovement()
    {
        //if (isDead || isPerformingAction)
        //{
        //    StopMoving();
        //    return;
        //}

        CalculatePath();
        Vector3 moveDir = SetMoveDirection();
        if (moveDir == Vector3.zero)
        {
            Debug.LogWarning("WARNING: Direction Is Zero");
            moveDir = transform.forward;
        }

        bossMovement.SetMoveDirection(moveDir);
        bossMovement.HandleAllMovement(bossMovement.Speed);
    }

    public void AnimateMovement()
    {
        bool moving = movement.Velocity.magnitude > 0.1f;
        animator.SetBool("moving", moving);
        //animator.SetFloat("Speed", sprinting ? 1f : 0f);

    }

    public bool IsTargetInAttackDistance()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= stoppingDistance;
    }

    protected override void OnTookDamage()
    {
        if (!IsOwner || isDead || isStunned)
            return;

        hitCount++;
        if (hitCount >= poise)
        {
            stateMachine.SwitchState(new BoraxStunnedState(stateMachine, this));
            isStunned = true;
        }
    }

    protected override void OnDied()
    {
        if (!IsOwner || isDead)
            return;

        stateMachine.SwitchState(new BoraxDeadState(stateMachine, this));
    }
}

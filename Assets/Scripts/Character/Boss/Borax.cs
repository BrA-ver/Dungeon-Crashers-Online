using UnityEngine;

public class Borax : Boss
{
    [SerializeField] bool sprint;

    [SerializeField] Animator animator;

    [SerializeField, Range(1, 10)] int maxAttacks = 5;
    int attackCount = 0;

    [Header("Fireball")]
    [SerializeField] float fireballCooldown = 4f;
    float fireballCounter = 0f;
    bool canDoFireball;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        attackCount = maxAttacks;
    }

    protected override void Update()
    {
        if (!IsOwner)
            return;

        if (target == null)
            GetPlayerAsTarget();

        fireballCounter -= Time.deltaTime;
        if (fireballCounter <= 0f)
        {
            canDoFireball = true;
        }

        if (Vector3.Distance(target.transform.position, transform.position) <= stoppingDistance)
        {
            StopMoving();

            if (!isPerformingAction)
            {
                if (attackCount >= maxAttacks)
                {
                    // Scream
                    combatManager.Taunt();
                    attackCount = 0;
                }
                else
                {
                    if (canDoFireball)
                    {
                        // DO Fireball
                        animator.CrossFade("Attack_Fireball", 0.1f);
                        isPerformingAction = true;

                        fireballCounter = fireballCooldown;
                        attackCount++;
                        canDoFireball = false;
                    }
                    else
                    {

                        combatManager.Attack();
                        attackCount++;
                    }
                }
            }
            AnimateMovement(sprint);
            return;
        }

        float speed = sprint ? bossMovement.SprintSpeed: bossMovement.Speed;
        HandleMovement(speed);

        AnimateMovement(sprint);
    }

    protected virtual void HandleMovement(float moveSpeed)
    {
        if (isDead || isPerformingAction)
        {
            StopMoving();
            return;
        }

        CalculatePath();
        Vector3 moveDir = SetMoveDirection();
        if (moveDir == Vector3.zero)
        {
            Debug.LogWarning("WARNING: Direction Is Zero");
            moveDir = transform.forward;
        }

        bossMovement.SetMoveDirection(moveDir);
        bossMovement.HandleAllMovement(moveSpeed);
    }

    protected void AnimateMovement(bool sprinting)
    {
        bool moving = movement.Velocity.magnitude > 0.1f;
        animator.SetBool("moving", moving);
        animator.SetFloat("Speed", sprinting ? 1f : 0f);

    }
}

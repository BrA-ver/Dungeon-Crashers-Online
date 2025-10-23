using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [Header("Navigation")]
    [SerializeField] protected float stoppingDistance = 1.5f;
    [SerializeField] protected float pathUpdateRate = 0.5f;
    protected float updateCounter;
    protected NavMeshPath path;
    protected int currentCorner = 0;

    Player target;

    Hitbox hitbox;

    protected override void Awake()
    {
        base.Awake();

        hitbox = GetComponentInChildren<Hitbox>();
        hitbox.OnTookDamage.AddListener(OnTookDamage);
    }

    protected override void Start()
    {
        base.Start();
        path = new NavMeshPath();
        health.onDied.AddListener(OnDied);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        hitbox.OnTookDamage.RemoveListener(OnTookDamage);
        health.onDied.RemoveListener(OnDied);
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        if (target == null)
            target = FindObjectOfType<Player>();

        if (Vector3.Distance(target.transform.position, transform.position) <= stoppingDistance)
        {
            movement.SetMoveDirection(Vector3.zero);
            movement.HandleAllMovement();
            AnimateMovement();
            return;
        }

        CalculatePath();
        Vector3 moveDir = SetMoveDirection();
        if (moveDir == Vector3.zero)
        {
            Debug.LogWarning("WARNING: Direction Is Zero");
            moveDir = transform.forward;
        }

        movement.SetMoveDirection(moveDir);
        movement.HandleAllMovement();

        AnimateMovement();
    }

    protected void CalculatePath()
    {
        if (target == null) return;

        updateCounter -= Time.deltaTime;
        if (updateCounter <= 0f)
        {
            updateCounter = pathUpdateRate;// reset the counter

            NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);
            currentCorner = 0;
        }
    }

    protected Vector3 SetMoveDirection()
    {
        if (currentCorner >= path.corners.Length) return Vector3.zero;

        Vector3 corner = path.corners[currentCorner];
        Vector3 dir = (corner - transform.position);
        dir.y = 0f; // keep movement flat

        // If close to the current corner, move to the next one
        if (dir.magnitude < 0.5f)
        {
            currentCorner++;
            return Vector3.zero;
        }

        dir.Normalize();
        return dir;
    }

    void OnTookDamage()
    {
        if (IsOwner)
        {
            if (isDead) return;

            Debug.Log("Took Damage");
            animationHandler.PlayTargetAnimation("Hit");
        }
    }

    void OnDied()
    {
        if (IsOwner)
        {
            isDead = true;
            animationHandler.SetDeadParam(isDead);
        }
    }
}

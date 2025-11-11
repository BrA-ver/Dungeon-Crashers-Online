using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
public class Player : Character
{
    protected bool isActive = true;
    protected PlayerAnimationHandler playerAnimationHandler;
    protected PlayerMovement playerMovement;
    protected GroundCheck groundCheck;

    public CharacterController controller;

    public GroundCheck GroundCheck => groundCheck;
    public PlayerMovement PlayerMovement => playerMovement;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        playerAnimationHandler = animationHandler as PlayerAnimationHandler;
        playerMovement = movement as PlayerMovement;


        health = GetComponent<PlayerHealth>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        movement = GetComponent<PlayerMovement>();
        combatManager = GetComponent<PlayerCombatManager>();
    }

    protected override void Start()
    {
        base.Start();
        DontDestroyOnLoad(this.gameObject);
        //movement.enabled = false;
        SceneManager.activeSceneChanged += OnSceneChange;

        //CheckpointManager.instance.AddPlayer(this);
        //CheckpointManager.instance.MoveToCheckpoint(this);

        if (IsOwner)
            UIManager.instance.playerHealthBar.GetPlayer(this);

        GameManager.instance.AddPlayer(this);
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        Debug.Log("Change");
        if (newScene.buildIndex != 0)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }

    //protected virtual void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);

    //    movement = GetComponent<PlayerMovement>();
    //    networkManager = GetComponent<PlayerNetworkManager>();
    //    controller = GetComponent<CharacterController>();
    //    animationHandler = GetComponent<PlayerAnimationHandler>();
    //    combatManager = GetComponent<PlayerCombatManager>();
    //    health = GetComponent<PlayerHealth>();
    //    groundCheck = GetComponentInChildren<GroundCheck>();
    //}

    protected virtual void OnEnable()
    {
        InputHandler.instance.onAttackPress += OnAttack;
        InputHandler.instance.onJumpPress += OnJump;
        InputHandler.instance.onLockOnPressed += OnLockOn;
        health.onTookDamage.AddListener(OnTookDamage);
        health.onDied.AddListener(OnDied);
    }

    

    protected virtual void OnDisable()
    {
        InputHandler.instance.onAttackPress -= OnAttack;
        InputHandler.instance.onJumpPress -= OnJump;
        InputHandler.instance.onLockOnPressed -= OnLockOn;
        health.onTookDamage.RemoveListener(OnTookDamage);
        health.onDied.RemoveListener(OnDied);

    }

    

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!IsOwner)
            return;

        if (!isActive)
            return;

        if (!isPerformingAction && !isDead)
        {
            movement.HandleAllMovement();

            AnimateMovement();
        }
        else
        {
            if (movement.dashing)
            {
                movement.HandleDash();
            }
        }
    }

    protected override void AnimateMovement()
    {
        base.AnimateMovement();
        bool moving = movement.Velocity.magnitude > 0.1f;
        bool onGround = groundCheck.OnGround();

        playerAnimationHandler.SetMoveParameter(moving);
        playerAnimationHandler.SetGroundedParameter(onGround);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            PlayerCamera.instance.SetPlayer(this);
            Debug.Log(InputHandler.instance.gameObject.name);
        }
    }

    protected virtual void OnAttack()
    {
        if (isPerformingAction || !groundCheck.OnGround() || isDead)
            return;
        if (!IsOwner)
            return;

        isPerformingAction = true;
        combatManager.Attack();
    }

    protected virtual void OnJump()
    {
        if (isPerformingAction || !groundCheck.OnGround())
            return;
        if (!IsOwner)
            return;

        movement.Jump(groundCheck.OnGround());
    }

    private void OnLockOn()
    {
        if (!IsOwner)
            return;

        bool lockedOn = PlayerCamera.instance.lockOnCam.lockedOn;

        if (lockedOn)
        {
            PlayerCamera.instance.lockOnCam.StopLockOn();
        }
        else
        {
            PlayerCamera.instance.lockOnCam.LockOn();
        }
    }

    #region Health Events
    private void OnTookDamage()
    {
        if (isDead) return;

        combatManager.GetHit();
    }

    private void OnDied()
    {
        if (!IsOwner)
            return;

        if (isDead) return;

        isDead = true;
        playerAnimationHandler.SetDeadParam(isDead);
    }
    #endregion
}

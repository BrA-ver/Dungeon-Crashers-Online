using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerNetworkManager))]
public class Player : NetworkBehaviour
{
    // Syncing
    protected PlayerNetworkManager networkManager;

    protected PlayerMovement movement;
    protected PlayerAnimationHandler animationHandler;
    protected PlayerCombatManager combatManager;
    protected GroundCheck groundCheck;

    public CharacterController controller;

    public bool isPerformingAction = false;

    public PlayerAnimationHandler AnimationHandler => animationHandler;
    public PlayerNetworkManager PlayerNetworkManager => networkManager;
    public PlayerMovement PlayerMovement => movement;

    public GroundCheck GroundCheck => groundCheck;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        movement = GetComponent<PlayerMovement>();
        networkManager = GetComponent<PlayerNetworkManager>();
        controller = GetComponent<CharacterController>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        combatManager = GetComponent<PlayerCombatManager>();
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    protected virtual void OnEnable()
    {
        InputHandler.instance.onAttackPress += OnAttack;
        InputHandler.instance.onJumpPress += OnJump;
    }

    protected virtual void OnDisable()
    {
        InputHandler.instance.onAttackPress -= OnAttack;
        InputHandler.instance.onJumpPress -= OnJump;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsOwner)
        {
            //Debug.Log(networkManager.NetworkPosition.Value != null);
            networkManager.NetworkPosition.Value = transform.position;
            networkManager.NetworkRotation.Value = transform.rotation;
            networkManager.IsMoving.Value = movement.Velocity.magnitude > 0.1f;
            networkManager.OnGround.Value = groundCheck.OnGround();
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, networkManager.NetworkPosition.Value, ref networkManager.VelocityRef, networkManager.smoothTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkManager.NetworkRotation.Value, networkManager.rotSmoothTime);
            animationHandler.SetMoveParameter(networkManager.IsMoving.Value);
            animationHandler.SetGroundedParameter(networkManager.OnGround.Value);
        }

        if (!IsOwner)
            return;

        if (!isPerformingAction)
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

    protected virtual void AnimateMovement()
    {
        bool moving = movement.Velocity.magnitude > 0.1f;
        bool onGround = groundCheck.OnGround();

        animationHandler.SetMoveParameter(moving);
        animationHandler.SetGroundedParameter(onGround);
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
        if (isPerformingAction || !groundCheck.OnGround())
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
}

using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CharacterMovement))]
public class Character : NetworkBehaviour
{
    protected bool isDead;

    protected CharacterMovement movement;
    protected CharacterAnimationHandler animationHandler;
    protected CharacterNetworkManager networkManager;
    protected Health health;

    public CharacterMovement Movement => movement;
    public CharacterAnimationHandler AnimationHandler => animationHandler;
    public CharacterNetworkManager NetworkManager => networkManager;


    protected virtual void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        animationHandler = GetComponent<CharacterAnimationHandler>();
        networkManager = GetComponent<CharacterNetworkManager>();
        health = GetComponent<Health>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if (IsOwner)
        {
            //Debug.Log(networkManager.NetworkPosition.Value != null);
            networkManager.NetworkPosition.Value = transform.position;
            networkManager.NetworkRotation.Value = transform.rotation;
            networkManager.NetworkCurrentHealth.Value = health.CurrentHealth;
            networkManager.NetworkIsDead.Value = isDead;
            //networkManager.IsMoving.Value = movement.Velocity.magnitude > 0.1f;
            //networkManager.OnGround.Value = groundCheck.OnGround();
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, networkManager.NetworkPosition.Value, ref networkManager.VelocityRef, networkManager.smoothTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkManager.NetworkRotation.Value, networkManager.rotSmoothTime);
            health.CurrentHealth = networkManager.NetworkCurrentHealth.Value;
            animationHandler.SetDeadParam(networkManager.NetworkIsDead.Value);
            //animationHandler.SetMoveParameter(networkManager.IsMoving.Value);
            //animationHandler.SetGroundedParameter(networkManager.OnGround.Value);
        }
    }

    protected virtual void AnimateMovement()
    {
        bool moving = movement.Velocity.magnitude > 0.1f;
        animationHandler.SetMoveParameter(moving);
    }
}

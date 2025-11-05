using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CharacterMovement))]
public class Character : NetworkBehaviour
{
    public bool isDead;
    public bool isPerformingAction;

    protected CharacterMovement movement;
    protected CharacterAnimationHandler animationHandler;
    protected CharacterCombatManager combatManager;
    protected Health health;

    public CharacterMovement Movement => movement;
    public CharacterAnimationHandler AnimationHandler => animationHandler;
    public Health Health => health;


    protected virtual void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        animationHandler = GetComponent<CharacterAnimationHandler>();
        combatManager = GetComponent<CharacterCombatManager>();
        health = GetComponent<Health>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void AnimateMovement()
    {
        bool moving = movement.Velocity.magnitude > 0.1f;
        animationHandler.SetMoveParameter(moving);
    }

    
}

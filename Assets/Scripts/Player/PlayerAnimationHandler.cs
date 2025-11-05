using UnityEngine;
using Unity.Netcode;

public class PlayerAnimationHandler : CharacterAnimationHandler
{
    int grounded = Animator.StringToHash("onGround");

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetGroundedParameter(bool onGround)
    {
        animator.SetBool(grounded, onGround);
    }
}

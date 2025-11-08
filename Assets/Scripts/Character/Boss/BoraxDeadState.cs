using UnityEngine;

public class BoraxDeadState : BoraxState
{
    public BoraxDeadState(StateMachine stateMachine, Borax borax) : base(stateMachine, borax)
    {
    }

    public override void Enter()
    {
        base.Enter();
        borax.isDead = true;
        borax.animator.CrossFade("Die", 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        borax.StopMoving();
    }
}

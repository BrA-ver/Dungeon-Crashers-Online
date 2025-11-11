using UnityEngine;

public class BoraxStunnedState : BoraxState
{
    float stunCounter = 0f;

    public BoraxStunnedState(StateMachine stateMachine, Borax borax) : base(stateMachine, borax)
    {
    }

    public override void Enter()
    {
        base.Enter();
        borax.animator.CrossFade("Get Hit", 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        borax.StopMoving();

        stunCounter += deltaTime;
        if (stunCounter >= borax.stunTime)
        {
            borax.isPerformingAction = true;
            stateMachine.SwitchState(new BoraxIdleState(stateMachine, borax));
            borax.animator.CrossFade("Get Up", 0.1f);
            borax.isStunned = false;
            borax.hitCount = 0;
        }
    }
}

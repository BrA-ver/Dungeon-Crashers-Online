using UnityEngine;

public class BoraxIdleState : BoraxState
{
    public BoraxIdleState(StateMachine stateMachine, Borax borax) : base(stateMachine, borax)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Borax Idle Enter");
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Debug.Log("Borax Idle Tick");

        if (borax.isPerformingAction)
            return;

        borax.StopMoving();
        borax.AnimateMovement();

        if (borax.IsTargetInAttackDistance())
        {
            Debug.Log("Borax Attack");
            stateMachine.SwitchState(new BoraxAttackState(stateMachine, borax));
            return;
        }
        else
        {
            Debug.Log("Not In Attack Distance");
            // Move Towards Target
            stateMachine.SwitchState(new BoraxChaseState(stateMachine, borax));
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Borax Idle Exit");
    }

    
}

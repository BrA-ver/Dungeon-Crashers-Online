using UnityEngine;

public class BoraxAttackState : BoraxState
{
    public BoraxAttackState(StateMachine stateMachine, Borax borax) : base(stateMachine, borax)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (borax.Movement.dashing)
        {
            borax.Movement.HandleDash();
        }
        if (borax.isPerformingAction)
            return;

        if (!borax.IsTargetInAttackDistance())
        {
            stateMachine.SwitchState(new BoraxChaseState(stateMachine, borax));
        }
        else
        {
            borax.CombatManager.Attack();
        }
            
    }
}

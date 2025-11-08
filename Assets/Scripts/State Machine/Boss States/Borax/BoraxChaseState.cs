using UnityEngine;

public class BoraxChaseState : BoraxState
{
    public BoraxChaseState(StateMachine stateMachine, Borax borax) : base(stateMachine, borax)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Borax Chase Enter");
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        borax.HandleMovement();
        borax.AnimateMovement();

        if (borax.IsTargetInAttackDistance())
        {
            stateMachine.SwitchState(new BoraxAttackState(stateMachine, borax));
        }
    }
}

using UnityEngine;

public abstract class BossState : State
{
    protected BossState(StateMachine stateMachine) : base(stateMachine)
    {
    }
}

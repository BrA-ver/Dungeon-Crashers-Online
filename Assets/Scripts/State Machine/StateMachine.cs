using UnityEngine;
using Unity.Netcode;

public class StateMachine : NetworkBehaviour
{
    public State currentState;

    private void Update()
    {
        if (!IsOwner)
            return;

        currentState?.Tick(Time.deltaTime);
    }

    public void SwitchState(State newState)
    {
        if (!IsOwner)
            return;

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}

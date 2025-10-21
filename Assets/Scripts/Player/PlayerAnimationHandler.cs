using UnityEngine;
using Unity.Netcode;

public class PlayerAnimationHandler : MonoBehaviour
{
    Animator animator;
    Player player;

    int moveParam = Animator.StringToHash("moving");
    int grounded = Animator.StringToHash("onGround");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    public void SetMoveParameter(bool moving)
    {
        animator.SetBool(moveParam, moving);
    }
    public void SetGroundedParameter(bool onGround)
    {
        animator.SetBool(grounded, onGround);
    }

    public void PlayTargetAnimation(string targetAnimaion)
    {
        animator.CrossFade(targetAnimaion, 0.2f);
        if (player.IsOwner)
            player.PlayerNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimaion);
    }
}

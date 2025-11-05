using UnityEngine;
using Unity.Netcode;

public class CharacterAnimationHandler : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected Player player;

    int moveParam = Animator.StringToHash("moving");

    protected virtual void Awake()
    {
        if (animator == null)
        {
            Debug.LogWarning($"WARNING: Animation Handler of {gameObject.name} has no animator.");
        }
        player = GetComponent<Player>();
    }

    public void SetMoveParameter(bool moving)
    {
        animator.SetBool(moveParam, moving);
    }

    public void SetDeadParam(bool isDead)
    {
        animator.SetBool("isDead", isDead);
    }

    public void PlayTargetAnimation(string targetAnimaion)
    {
        animator.CrossFade(targetAnimaion, 0.1f);
        //if (player.IsOwner)
            //player.MyNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimaion);
    }
}

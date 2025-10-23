using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationHandler : MonoBehaviour
{
    protected Animator animator;
    protected Character character;

    int moveParam = Animator.StringToHash("moving");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
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
        if (character.IsOwner)
            character.NetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimaion);
    }
}

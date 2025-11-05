using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    protected Character character;

    protected string attackAnim;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    protected virtual void Start()
    {
        
    }

    public virtual void Attack()
    {
        character.isPerformingAction = true;
        character.AnimationHandler.PlayTargetAnimation(attackAnim);
    }

    public void GetHit()
    {
        character.isPerformingAction = true;
        character.AnimationHandler.PlayTargetAnimation("Hit");
    }

    public void RecoverFromHit()
    {
        character.isPerformingAction = false;
    }

    public virtual void FinishAttack()
    {
        character.isPerformingAction = false;
    }
}

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

    public virtual void AttackDash()
    {

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

    public void Taunt()
    {
        character.isPerformingAction = true;
        character.AnimationHandler.PlayTargetAnimation("Taunt");
    }

    public virtual void StartRotation()
    {

    }
    
    public virtual void StopRotation()
    {

    }
}

[System.Serializable]
public class Attack
{
    public string animName;
    public float dashSpeed;
    public float duration;
}
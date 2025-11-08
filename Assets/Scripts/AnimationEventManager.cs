using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    [SerializeField] CharacterCombatManager combatManager;

    public void FinishAttack()
    {
        combatManager.FinishAttack();
    }

    public void RecoverFromHit()
    {
        combatManager.RecoverFromHit();
    }

    public void Dash()
    {
        combatManager.AttackDash();
    }
}

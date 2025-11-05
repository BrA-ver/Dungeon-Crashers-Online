using UnityEngine;

public class EnemyCombatManager : CharacterCombatManager
{
    [Header("Enemy Attack")]
    [SerializeField] EnemyAttack[] enemyAttacks;
    int totalWeight;

    protected override void Start()
    {
        base.Start();
        foreach (EnemyAttack attack in enemyAttacks)
        {
            totalWeight += attack.weight;
        }
    }

    public override void Attack()
    {
        // Choose Random Attack
        int randomWeight = Random.Range(1, totalWeight + 1);
        int temp = 0;

        foreach (EnemyAttack attack in enemyAttacks)
        {
            temp += attack.weight;

            if (randomWeight <= temp)
            {
                attackAnim = attack.attackName;
                break;
            }
        }

        base.Attack();
    }
}

[System.Serializable]
public class EnemyAttack
{
    public string attackName;
    public int weight;
}
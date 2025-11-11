using UnityEngine;

public class EnemyCombatManager : CharacterCombatManager
{
    [Header("Enemy Attack")]
    [SerializeField] EnemyAttack[] enemyAttacks;
    protected EnemyAttack currentAttack;
    int totalWeight;

    [SerializeField] float rotationSpeed = 6f;

    bool canRotate;

    Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = character as Enemy;
    }

    protected override void Start()
    {
        base.Start();
        foreach (EnemyAttack attack in enemyAttacks)
        {
            totalWeight += attack.weight;
        }
    }

    private void Update()
    {
        if (canRotate)
        {
            Player target = enemy.Target;

            Vector3 lookDir = target.transform.position - transform.position;
            lookDir.y = 0f;
            lookDir.Normalize();
            lookDir.y = 0f;
            lookDir.Normalize();

            HandleRotation(lookDir);
        }
    }

    public void HandleRotation(Vector3 lookDir)
    {
        if (lookDir.magnitude < 0.1f) return;

        Quaternion lookRotation = Quaternion.LookRotation(lookDir);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
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
                attackAnim = attack.animName;
                currentAttack = attack;
                break;
            }
        }

        base.Attack();
    }

    public override void StartRotation()
    {
        canRotate = true;
    }

    public override void StopRotation()
    {
        canRotate = false;
    }
}

[System.Serializable]
public class EnemyAttack: Attack
{
    [Header("Enemy")]
    public int weight;
}
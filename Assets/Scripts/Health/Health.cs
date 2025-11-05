using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    Character character;

    [SerializeField] protected int maxHealth = 10;
    [SerializeField] protected int currentHealth;

    [Header("Events")]
    public UnityEvent onTookDamage;
    public UnityEvent onDied;

    public float HealthRatio { get { return currentHealth / maxHealth; } }
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    protected void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        if (!character.IsOwner)
            return;

        if (currentHealth <= 0) return;

        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            onDied?.Invoke();
            return;
        }
        onTookDamage?.Invoke();
    }
}

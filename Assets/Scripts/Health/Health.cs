using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class Health : NetworkBehaviour
{
    Character character;

    [SerializeField] protected int maxHealth = 10;
    [SerializeField] protected int currentHealth;

    public Character attacker;

    // Server = writes | Everyone = reads
    public NetworkVariable<int> NetMaxHealth = new NetworkVariable<int>(
        10,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public NetworkVariable<int> NetCurrentHealth = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public NetworkVariable<float> NetHealthRatio = new NetworkVariable<float>(
        1f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    [Header("Events")]
    public UnityEvent onTookDamage;
    public UnityEvent onDied;
    public UnityEvent onRevived;

    public float HealthRatio { get; private set; }
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    protected virtual void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth = maxHealth;
            SetHealthRatio();

            NetMaxHealth.Value = maxHealth;
            NetCurrentHealth.Value = currentHealth;
            NetHealthRatio.Value = HealthRatio;
        }

        // Listen for sync updates
        NetCurrentHealth.OnValueChanged += OnHealthChanged;
        NetHealthRatio.OnValueChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        NetCurrentHealth.OnValueChanged -= OnHealthChanged;
        NetHealthRatio.OnValueChanged -= OnHealthChanged;
    }

    public void Revive()
    {
        if (!IsServer)
        {
            return;
        }

        currentHealth = maxHealth;
        onRevived?.Invoke();
        onTookDamage?.Invoke();
    }


    private void OnHealthChanged(int oldValue, int newValue)
    {
        currentHealth = newValue;
        onTookDamage?.Invoke();
    }

    private void OnHealthChanged(float oldValue, float newValue)
    {
        HealthRatio = newValue;
        onTookDamage?.Invoke();
    }

    public virtual void TakeDamage(int damage, Character attacker)
    {
        if (!IsServer)
        {
            // Only tell the server to apply the damage
            TakeDamageServerRpc(damage);
            this.attacker = attacker;
            return;
        }

        ApplyDamage(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    void TakeDamageServerRpc(int damage)
    {
        ApplyDamage(damage);
    }

    private void ApplyDamage(int damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        SetHealthRatio();

        // Sync via NetworkVariables (auto replicated to all clients)
        NetCurrentHealth.Value = currentHealth;
        NetHealthRatio.Value = HealthRatio;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //onDied?.Invoke(); // fire on server too, if needed
        DieClientRpc();
    }

    [ClientRpc]
    void DieClientRpc()
    {
        currentHealth = 0;
        HealthRatio = 0f;
        onDied?.Invoke();
    }

    void SetHealthRatio()
    {
        HealthRatio = (float)currentHealth / (float)maxHealth;
    }
}

using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class EnemySpawner : NetworkBehaviour
{
    [Header("Character")]
    [SerializeField] Enemy enemy;
    [SerializeField] Enemy spawnedEnemy;

    public UnityEvent<Character> OnCharacterSpawn;

    public Enemy Enemy => enemy;
    public Enemy SpawnedEnemy => spawnedEnemy;

    //NetworkVariable<Enemy> SpawnedEnemyNet = new NetworkVariable<Enemy>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        if (GameManager.instance.networkStarted)
        {
            SpawnCharacter();
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.instance.networkStarted)
        {
            SpawnCharacter();
            gameObject.SetActive(false);
        }
    }

    public void Spawn()
    {
        WorldAIManager.instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void SpawnCharacter()
    {
        if (enemy == null) return;
        spawnedEnemy = Instantiate(enemy);

        //if (IsOwner)
        //    SpawnedEnemyNet.Value = spawnedEnemy;
        //else
        //    spawnedEnemy = SpawnedEnemyNet.Value;

        spawnedEnemy.transform.position = transform.position;
        spawnedEnemy.transform.rotation = transform.rotation;
        spawnedEnemy.GetComponent<NetworkObject>().Spawn();
        OnCharacterSpawn?.Invoke(enemy);
        
    }


}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.Netcode;

public class Wave : NetworkBehaviour
{
    [SerializeField] protected List<EnemySpawner> spawners = new List<EnemySpawner>();
    public List<Enemy> enemies = new List<Enemy>();
    public event Action<string, string> onWaveStared;
    public event Action onEnemiesEnabled;
    public event Action onWaveEnded;

    public string WaveName = string.Empty;
    public string enemyNames = string.Empty;

    [SerializeField] protected float waveStartTime = 3f;

    int deadEnemyNum = 0;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            EnemySpawner spawner = child.GetComponent<EnemySpawner>();
            if (spawner != null)
            {
                spawners.Add(spawner);
                //spawner.onDied += OnEnemyDied;
                enemyNames += $"{spawner.Enemy.EnemyName}\n";
            }
            else
            {
                Debug.Log("Null");
            }
        }
    }

    

    //private void OnDisable()
    //{
    //    foreach (Enemy enemy in spawners)
    //    {
    //        enemy.onDied -= OnEnemyDied;
    //    }
    //}

    public virtual void StartWave()
    {
        if (!IsServer)
            return;
        StartCoroutine(StartWaveRoutine());
    }

    protected void OnEnemyDied()
    {
        if (!IsServer)
            return;
        deadEnemyNum++;
        bool allEnemiesDead = deadEnemyNum >= enemies.Count;
        Debug.Log("Dead Enemies: " + deadEnemyNum);
        if (allEnemiesDead)
        {
            StartCoroutine(EndWaveRoutine());
        }
    }

    protected virtual IEnumerator StartWaveRoutine()
    {
        Debug.Log("Wave Routine");
        // 1. Disable Player Controls

        // 2. Display wave names on screen

        foreach (EnemySpawner spawner in spawners)
        {
            spawner.gameObject.SetActive(true);
            spawner.Spawn();

            Enemy spawnedCharacter = spawner.SpawnedEnemy as Enemy;
            
            // Add the spawned enemy to the enemies list and it's name
            enemies.Add(spawnedCharacter);

            spawnedCharacter.enabled = false;

            spawnedCharacter.Health.onDied.AddListener(OnEnemyDied);
            Debug.Log("Character Spawned");
        }
        InvokeWaveStartedClientRpc(WaveName, enemyNames);

        yield return new WaitForSeconds(waveStartTime);

        // 3. Hide the wave display

        foreach (Enemy enemy in enemies)
        {
            enemy.enabled = true;
        }

        InvokeEnemiesEnabledClientRpc();
        // 4. Enable Player Controls
    }

    [ClientRpc]
    protected void InvokeWaveStartedClientRpc(string waveName, string enemyNames)
    {
        onWaveStared?.Invoke(waveName, enemyNames);
    }

    [ClientRpc]
    protected void InvokeEnemiesEnabledClientRpc()
    {
        onEnemiesEnabled?.Invoke();
    }

    protected virtual IEnumerator EndWaveRoutine()
    {
        yield return new WaitForSeconds(2f);

        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();

        yield return new WaitForSeconds(2f);

        onWaveEnded?.Invoke();
        Debug.Log("End of Wave");
    }
}

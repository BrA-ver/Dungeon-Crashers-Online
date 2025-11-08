using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class WaveSpawner : NetworkBehaviour
{
    public static WaveSpawner instance;

    public List<Wave> waves = new List<Wave>();
    int currentWave = 0;

    [SerializeField] float timeBetweenWaves;

    [SerializeField] Button startWaveButton;

    public event Action onBattleStarted;
    public event Action onBattleEnded;
    public event Action onHideDisplay;

    bool first = true;

    private void Awake()
    {
        instance = this;
        foreach (Transform child in transform)
        {
            Wave wave = child.GetComponent<Wave>();
            if (wave != null)
            {
                waves.Add(wave);
                wave.onWaveEnded += OnWaveEnded;
            }
        }
    }

    // Initialize waves in start because the waves initialize enemies in awake
    private void Start()
    {
        

        startWaveButton.onClick.AddListener(StartWave);

        //StartWave();
    }

    void StartWave()
    {
        if (!IsServer)
            return;

        

        StartCoroutine(StartBattleRoutine());
        
    }

    [ServerRpc]
    void StartWaveServerRpc()
    {
        StartWaveClientRpc();
    }

    [ClientRpc]
    void StartWaveClientRpc()
    {
        waves[currentWave].StartWave();
        Debug.Log("Wave Started");
    }

    void OnWaveEnded()
    {
        WaveEndRoutine();
    }

    void WaveEndRoutine()
    {

        Debug.Log("Wave Ended");
        currentWave++;
        if (currentWave >= waves.Count)
        {
            StartCoroutine(EndBattleRoutine());
            return;
        }

        // Start the next wave
        StartWave();
    }

    IEnumerator StartBattleRoutine()
    {
        if (first)
            onBattleStarted?.Invoke();
        first = false;

        yield return new WaitForSeconds(1f);

        onHideDisplay?.Invoke();

        yield return new WaitForSeconds(1f);

        

        if (currentWave >= waves.Count) 
            yield return null;

        StartWaveServerRpc();
        
    }

    IEnumerator EndBattleRoutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Batlle Over");
        onBattleEnded?.Invoke();

        yield return new WaitForSeconds(2f);
        onHideDisplay?.Invoke();
    }
}

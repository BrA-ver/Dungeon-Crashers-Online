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

        if (currentWave >= waves.Count) return;

        StartWaveServerRpc();
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
            Debug.Log("Batlle Over");
            return;
        }

        // Start the next wave
        StartWave();
    }
}

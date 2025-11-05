using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] playerPrefabs;
    int currentPrefab;

    [SerializeField] List<GameObject> cleintObjects;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.NetworkConfig.AutoSpawnPlayerPrefabClientSide = false;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        currentPrefab++;
        if (currentPrefab >= playerPrefabs.Length)
        {
            currentPrefab = 0;
        }
        GameObject current = playerPrefabs[currentPrefab];
        
        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = current;
    }
}
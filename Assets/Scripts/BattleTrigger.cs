using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleTrigger : NetworkBehaviour
{
    [SerializeField] private float targetHeight = 10f;
    [SerializeField] private float lerpTime = 5f;
    [SerializeField] private string targetScene = "World";

    [SerializeField] Collider _collider;

    private Vector3 startPos, targetPos;
    private bool moving = false;

    private List<ulong> detectedClientIds = new List<ulong>();

    private void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + new Vector3(0f, targetHeight, 0f);

        _collider.enabled = false;
        Invoke(nameof(EnableCollider), 1.5f);
    }

    private void EnableCollider()
    {
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect locally on any client or host
        Player player = other.GetComponentInParent<Player>();
        if (player == null)
            return;

        // Tell the server that this player entered
        NotifyPlayerEnteredServerRpc(player.OwnerClientId);
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player == null)
            return;

        NotifyPlayerExitedServerRpc(player.OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyPlayerEnteredServerRpc(ulong clientId)
    {
        NotifyPlayerEnteredClientRpc(clientId);
    }

    [ClientRpc]
    private void NotifyPlayerEnteredClientRpc(ulong clientId)
    {
        if (!detectedClientIds.Contains(clientId))
        {
            detectedClientIds.Add(clientId);
            Debug.Log($"Player {clientId} entered. Total: {detectedClientIds.Count}");

            // If all players are inside, start the battle
            if (detectedClientIds.Count >= NetworkManager.Singleton.ConnectedClientsList.Count && !moving)
            {
                Debug.Log("All players inside. Starting battle...");
                BeginBattleClientRpc();
                gameObject.SetActive(false);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyPlayerExitedServerRpc(ulong clientId)
    {
        if (detectedClientIds.Contains(clientId))
        {
            detectedClientIds.Remove(clientId);
            Debug.Log($"Player {clientId} exited.");
        }
    }

    [ClientRpc]
    private void BeginBattleClientRpc()
    {
        if (WaveSpawner.instance != null)
        {
            Debug.Log("Battle started on client!");
            WaveSpawner.instance.StartWave();
        }
    }

    private IEnumerator MoveElevatorRoutine()
    {
        moving = true;
        float timePassed = 0f;

        while (timePassed < lerpTime)
        {
            timePassed += Time.deltaTime;
            float t = timePassed / lerpTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        NetworkManager.SceneManager.LoadScene(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}

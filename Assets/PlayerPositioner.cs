using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class PlayerPositioner : NetworkBehaviour
{
    public Transform[] positions;

    void Start()
    {
        if (!IsServer)
            return;

        RequestRepositionServerRpc();
    }

    [ServerRpc]
    private void RequestRepositionServerRpc()
    {
        Debug.Log("Positioning");
        SetPlayerPositionClientRpc();
    }

    // This runs only on the server

    [ClientRpc]
    private void SetPlayerPositionClientRpc()
    {
        List<Player> players = GameManager.instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            if (i >= positions.Length) break;

            var p = players[i];
            if (p == null) continue;

            p.controller.enabled = false;
            p.transform.position = positions[i].position;
            p.transform.forward = positions[i].forward;
            p.controller.enabled = true;
        }
    }
}

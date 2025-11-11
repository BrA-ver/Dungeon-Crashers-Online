using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class Elevator : NetworkBehaviour
{
    [SerializeField] private float targetHeight = 10f;
    [SerializeField] private float lerpTime = 5f;
    [SerializeField] string targetScene = "World";

    private Vector3 startPos, targetPos;
    private bool moving = false;

    private List<Player> detectedPlayers = new List<Player>();

    private void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + new Vector3(0f, targetHeight, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        Player player = other.GetComponentInParent<Player>();
        if (player != null && !detectedPlayers.Contains(player))
        {
            detectedPlayers.Add(player);
            player.NetworkObject.TrySetParent(NetworkObject, true);
            player.GetComponent<PlayerMovement>().enabled = false; // disable movement while on elevator
        }

        // start moving if all players are in
        if (detectedPlayers.Count >= 2 && !moving)
        {
            StartCoroutine(MoveElevatorRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer)
            return;

        Player player = other.GetComponentInParent<Player>();
        if (player != null && detectedPlayers.Contains(player))
        {
            player.NetworkObject.TryRemoveParent(true);
            player.GetComponent<PlayerMovement>().enabled = true;
            detectedPlayers.Remove(player);
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

        // Unparent players before transition
        foreach (var player in detectedPlayers)
        {
            player.NetworkObject.TryRemoveParent(true);
            player.GetComponent<PlayerMovement>().enabled = true;
        }

        // Proper way to change scenes in Netcode
        NetworkManager.SceneManager.LoadScene(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}

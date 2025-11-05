using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    [SerializeField] List<Checkpoint> checkpoints;
    [SerializeField] List<Player> players;
    int index;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanage;
    }

    private void OnSceneChanage(Scene oldScene, Scene newScene)
    {
        checkpoints.Clear();
        Checkpoint[] checks = FindObjectsByType<Checkpoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (Checkpoint point in checks)
        {
            checkpoints.Add(point);
        }

        if (players.Count <= 0)
            return;

        foreach (Player player in players)
        {
            MoveToCheckpoint(player);
        }
    }

    public void MoveToCheckpoint(Player player)
    {
        player.controller.enabled = false;
        player.transform.position = checkpoints[index].transform.position;
        player.controller.enabled = true;
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}

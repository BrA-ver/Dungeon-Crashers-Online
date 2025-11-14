using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public bool networkStarted;

    //NetworkVariable<List<Player>> activePlayers = new NetworkVariable<List<Player>>(
    //    new List<Player>(), 
    //    NetworkVariableReadPermission.Everyone, 
    //    NetworkVariableWritePermission.Owner);

    public List<Player> players = new List<Player>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStated;
        SceneManager.activeSceneChanged += OnActiveSceneChanges;
    }

    private void OnActiveSceneChanges(Scene oldScene, Scene newScene)
    {
        if(newScene.buildIndex == 1)
        {
            AudioManager.instance.PlayMusic("Lobby");
        }
    }

    private void OnServerStated()
    {
        networkStarted = true;
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
        //activePlayers.Value = players;
    }


}

using Unity.Netcode;
using Unity.Netcode.Transports.UTP; // <--- Important
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectUIScript : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private LanDiscovery lanDiscovery;

    private void Start()
    {
        hostButton.onClick.AddListener(HostButtonOnClick);
        clientButton.onClick.AddListener(ClientButtonOnClick);
    }

    private void HostButtonOnClick()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started!");
            lanDiscovery.BroadcastHost(); // start broadcasting!
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            CheckBeginOnline();
        }
        else
        {
            Debug.LogError("Failed to start host!");
        }
    }


    private void ClientButtonOnClick()
    {
        lanDiscovery.StartListening();
        string ip;

#if UNITY_EDITOR
        // When testing in Unity multiplayer play mode, force localhost
        ip = "127.0.0.1";
#else
    // In actual LAN play, use discovered IP
    if (lanDiscovery.detectedHostIP == null)
    {
        Debug.LogError("No host detected on LAN.");
        return;
    }
    ip = lanDiscovery.detectedHostIP;
#endif

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ip;

        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log($"Client started! Connecting to {ip}");
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
        else
        {
            Debug.LogError("Failed to start client!");
        }
    }


    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
        CheckBeginOnline();
    }

    private void CheckBeginOnline()
    {
        if (NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClients.Count >= 2)
        {
            BeginOnline();
        }
    }

    private void BeginOnline()
    {
        const string targetScene = "World";

        int buildIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + targetScene + ".unity");
        Debug.Log($"Loading {targetScene}, Build Index: {buildIndex}");

        if (buildIndex == -1)
        {
            Debug.LogError($"Scene '{targetScene}' not found in build settings!");
            return;
        }

        Debug.Log("Both players connected. Starting game...");
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }
}

using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool networkStarted;

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
    }

    private void OnServerStated()
    {
        networkStarted = true;
    }
}

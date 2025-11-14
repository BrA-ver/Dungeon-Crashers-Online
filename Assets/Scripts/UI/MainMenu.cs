using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayMusic("Main Menu");
    }

    public void PlayAsHost()
    {
        //NetworkManager.Singleton.StartHost();
        //SceneManager.LoadScene(2);

        //// Start hosting
        //if (NetworkManager.Singleton.StartHost())
        //{
        //    // Load the game scene through Netcode's scene manager
        //    NetworkManager.Singleton.SceneManager.LoadScene(
        //        "World", // or SceneManager.GetSceneByBuildIndex(1).name
        //        LoadSceneMode.Single
        //    );
        //}

        Debug.Log("=== HOST button pressed ===");
        bool started = NetworkManager.Singleton.StartHost();
        Debug.Log("Host started: " + started);
        if (started)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("World", LoadSceneMode.Single);
        }
    }

    public void PlayAsClient()
    {
        Debug.Log("Trying to start client...");
        bool started = NetworkManager.Singleton.StartClient();
        Debug.Log("StartClient() returned: " + started);
    }
}

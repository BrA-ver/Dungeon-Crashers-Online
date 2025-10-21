using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
        SceneManager.LoadScene(1);
    }

    public void StartGameAsClient()
    {
        NetworkManager.Singleton.Shutdown(); // Might not need this

        NetworkManager.Singleton.StartClient();
    }
}

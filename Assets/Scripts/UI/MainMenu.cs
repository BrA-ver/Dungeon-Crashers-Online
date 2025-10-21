using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayAsHost()
    {
        NetworkManager.Singleton.StartHost();
        SceneManager.LoadScene(1);
    }

    public void PlayAsClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}

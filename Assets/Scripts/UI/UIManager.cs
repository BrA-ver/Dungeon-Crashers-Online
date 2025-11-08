using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    public static UIManager instance;

    [SerializeField] Button hostButton, clientButton;

    [SerializeField] RectTransform healthBarHolder;
    [SerializeField] BossHealthBar healthBar;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        hostButton.onClick.AddListener( () => 
        {
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    public void ShowBossHealth(Boss boss)
    {
        BossHealthBar bossHealth = Instantiate(healthBar, healthBarHolder);

        bossHealth.GetBoss(boss);
    }
}

using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    public static UIManager instance;

    [SerializeField] BattleDisplay battleDisplay;
    [SerializeField] WaveDisplay waveDisplay;

    [SerializeField] Button hostButton, clientButton;

    [SerializeField] RectTransform healthBarHolder;
    [SerializeField] BossHealthBar healthBar;

    public PlayerHealthBar playerHealthBar;

    public BattleDisplay BattleDisplay => battleDisplay;
    public WaveDisplay WaveDisplay => waveDisplay;

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
        hostButton.onClick.AddListener( () => 
        {
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });

        battleDisplay = GetComponentInChildren<BattleDisplay>();
    }

    public void ShowBossHealth(Boss boss)
    {
        BossHealthBar bossHealth = Instantiate(healthBar, healthBarHolder);

        bossHealth.GetBoss(boss);
    }
}

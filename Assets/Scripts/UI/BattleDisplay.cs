using UnityEngine;
using TMPro;

public class BattleDisplay : MonoBehaviour
{
    WaveSpawner spawner;

    [SerializeField] GameObject displayHolder;
    [SerializeField] TextMeshProUGUI battleText;

    private void Start()
    {
        spawner = WaveSpawner.instance;
        spawner.onBattleStarted += OnBattleStarted;
        spawner.onBattleEnded += OnBattleEnded;
        spawner.onHideDisplay += OnHideDisplay;
    }

    private void OnDisable()
    {
        spawner.onBattleStarted -= OnBattleStarted;
        spawner.onBattleEnded -= OnBattleEnded;
        spawner.onHideDisplay -= OnHideDisplay;
    }

    private void OnBattleStarted()
    {
        displayHolder.gameObject.SetActive(true);
        battleText.text = "Battle Start!!";
    }

    private void OnBattleEnded()
    {
        displayHolder.gameObject.SetActive(true);
        battleText.text = "Victory!!";
    }

    private void OnHideDisplay()
    {
        displayHolder.SetActive(false);
    }
}

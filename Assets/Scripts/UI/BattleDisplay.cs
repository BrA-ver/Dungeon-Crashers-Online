using UnityEngine;
using TMPro;
using Unity.Netcode;

public class BattleDisplay : NetworkBehaviour
{
    WaveSpawner spawner;

    [SerializeField] GameObject displayHolder;
    [SerializeField] TextMeshProUGUI battleText;

    private void OnDisable()
    {
        if (spawner != null)
        {
            spawner.onBattleStarted -= OnBattleStarted;
            spawner.onBattleEnded -= OnBattleEnded;
            spawner.onHideDisplay -= OnHideDisplay;
        }
    }

    public void GetWaveSpawner(WaveSpawner spawner)
    {

        Debug.Log("Got Spawner");
        this.spawner = spawner;
        spawner.onBattleStarted += OnBattleStarted;
        spawner.onBattleEnded += OnBattleEnded;
        spawner.onHideDisplay += OnHideDisplay;
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

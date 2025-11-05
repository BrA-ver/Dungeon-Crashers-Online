using UnityEngine;
using TMPro;
public class WaveDisplay : MonoBehaviour
{
    [SerializeField] GameObject bg;
    [SerializeField] TextMeshProUGUI waveName;
    [SerializeField] TextMeshProUGUI enemyNames;

    private void Start()
    {
        foreach(Wave wave in WaveSpawner.instance.waves)
        {
            wave.onWaveStared += ShowWaveDisplay;
            wave.onEnemiesEnabled += HideWaveDisplay;
        }
    }

    public void ShowWaveDisplay(string name, string enemies)
    {

        //Debug.Log(name);
        bg.SetActive(true);
        waveName.text = name;
        enemyNames.text = enemies;
    }

    public void HideWaveDisplay()
    {
        waveName.text = string.Empty;
        enemyNames.text = string.Empty;
        bg.SetActive(false);
    }
}

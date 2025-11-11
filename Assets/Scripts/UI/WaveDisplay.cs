using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class WaveDisplay : MonoBehaviour
{
    [SerializeField] GameObject bg;
    [SerializeField] TextMeshProUGUI waveName;
    [SerializeField] TextMeshProUGUI enemyNames;

    

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

    public void GetWaves(List<Wave> waves)
    {
        foreach (Wave wave in waves)
        {
            wave.onWaveStared += ShowWaveDisplay;
            wave.onEnemiesEnabled += HideWaveDisplay;
            Debug.Log("Subscribed");
        }
    }
}

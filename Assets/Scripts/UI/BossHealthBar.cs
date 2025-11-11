using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI nameText;

    Health health;

    public void GetBoss(Boss boss)
    {
        health = boss.Health;
        health.onTookDamage.AddListener(OnBossTookDamage);
        health.onDied.AddListener(OnBossDied);

        nameText.text = boss.EnemyName;
    }

    

    private void OnBossTookDamage()
    {
        Debug.Log("Health Bar Change");
        Debug.Log(health.NetHealthRatio.Value);
        healthSlider.value = health.HealthRatio;
    }

    private void OnBossDied()
    {
        Debug.Log(health.NetHealthRatio.Value);
        healthSlider.value = health.HealthRatio;

        Invoke(nameof(DeleteHealthBar), 1f);
    }

    private void DeleteHealthBar()
    {
        health.onTookDamage.RemoveListener(OnBossTookDamage);
        health.onDied.RemoveListener(OnBossDied);

        Destroy(gameObject);
    }
}

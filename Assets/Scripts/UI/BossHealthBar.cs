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
        health.onDied.AddListener(OnBossTookDamage);

        nameText.text = boss.EnemyName;
    }

    private void OnBossTookDamage()
    {
        healthSlider.value = health.HealthRatio;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI nameText;

    Health health;

    public void GetPlayer(Player player)
    {
        health = player.Health;
        health.onTookDamage.AddListener(OnBossTookDamage);
        health.onDied.AddListener(OnBossTookDamage);
    }

    private void OnBossTookDamage()
    {
        Debug.Log("Health Bar Change");
        Debug.Log(health.NetHealthRatio.Value);
        healthSlider.value = health.HealthRatio;
    }
}

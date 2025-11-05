using UnityEngine;

public class PlayerHealth : Health
{
    [Header("Player")]
    [SerializeField] float invincibilityTime = 2f;
    float invincibilityCounter;

    protected override void Update()
    {
        base.Update();

        if (invincibilityCounter > 0f)
        {
            invincibilityCounter = Mathf.Max(0f, invincibilityCounter - Time.deltaTime);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (invincibilityCounter > 0)
            return;
        //Debug.Log("Player Hit");
        invincibilityCounter = invincibilityTime;
        base.TakeDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    private PlayerAnimations playerAnimations;

    private void Awake()
    {
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    private void Update()
    {
        if(stats.Health <= 0f)
        {
            PlayerDead();
        }
    }

    public void TakeDamage(float amount)
    {
        if (stats.Health <= 0f) return;
        stats.Health -= amount;
        DamageManager.instance.ShowDamageText(amount, transform);
        if(stats.Health <= 0f)
        {
            stats.Health = 0f;
            PlayerDead();
        }
    }

    public bool CanRestoreHealth()
    {
        return stats.Health > 0f && stats.Health < stats.MaxHealth;
    }

    public void RestoreHealth(float amount)
    {
        stats.Health += amount;
        stats.Health = Mathf.Min(stats.Health, stats.MaxHealth);
    }

    private void PlayerDead()
    {
        // ½ÇÉ«ËÀÍö
        playerAnimations.SetDeadAnimation(); 
    }
}

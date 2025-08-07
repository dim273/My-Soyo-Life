using System;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public static event Action OnPlayerUpgradeEvent;

    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    [Header("Settings")]
    [SerializeField] private UpgradeSettings[] settings;

    private void UpgradePlayer(int index)
    {
        stats.BaseDamage += settings[index].DamageUpgrade;
        stats.TotalDamage += settings[index].DamageUpgrade;
        stats.MaxHealth += settings[index].HealthUpgrade;
        stats.Health = stats.MaxHealth;
        stats.MaxMana += settings[index].ManaUpgrade;
        stats.Mana = stats.MaxMana;
        stats.CriticalChance += settings[index].CChanceUpgrade;
        stats.CriticalDamage += settings[index].CDamageUpgrade;
    }

    private void AttributeCallback(AttributeType attributeType)
    {
        if (stats.AttributePoints == 0) return;

        switch(attributeType)
        {
            case AttributeType.Strength:
                UpgradePlayer(0);
                stats.Strength++;
                break;
            case AttributeType.Dexterity:
                UpgradePlayer(1);
                stats.Dexterity++; 
                break;
            case AttributeType.intelligence:
                UpgradePlayer(2);
                stats.Intelligence++;
                break;
        }
        stats.AttributePoints--;
        OnPlayerUpgradeEvent?.Invoke();
    }

    private void OnEnable()
    {
        AttributeButton.OnAttributeSelectedEvent += AttributeCallback;
    }

    private void OnDisable()
    {
        AttributeButton.OnAttributeSelectedEvent -= AttributeCallback;
    }
}

[Serializable]
public class UpgradeSettings
{
    public string Name;

    [Header("Config")]
    public float DamageUpgrade;
    public float HealthUpgrade;
    public float ManaUpgrade;
    public float CChanceUpgrade;
    public float CDamageUpgrade;
}

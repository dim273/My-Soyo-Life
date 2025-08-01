using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotionItem", menuName = "Items/Health Potion")]
public class HealthPotionItem : InventoryItem
{
    [Header("Config")]
    public float HealthValue;

    public override bool UseItem()
    {
        if (GameManager.instance.Player.PlayerHealth.CanRestoreHealth())
        {
            GameManager.instance.Player.PlayerHealth.RestoreHealth(HealthValue);
            return true;
        }
        return false;
    }
}

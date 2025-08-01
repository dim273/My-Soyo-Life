using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPotionItem", menuName = "Items/Mana Potion")]
public class ManaPotionItem : InventoryItem
{
    [Header("Config")]
    public float ManaValue;

    public override bool UseItem()
    {
        if(GameManager.instance.Player.PlayerMana.CanRestoreMana())
        {
            GameManager.instance.Player.PlayerMana.RestoreMana(ManaValue); 
            return true;
        }
        return false;
    }
}

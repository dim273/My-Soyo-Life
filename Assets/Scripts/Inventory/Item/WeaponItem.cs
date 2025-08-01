using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon", fileName = "WeaponItem")]
public class WeaponItem : InventoryItem
{
    [Header("Weapon")]
    public Weapon Weapon;
}

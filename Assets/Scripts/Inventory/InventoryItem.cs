using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Potion,
    Scroll,
    Ingredients,
    Treasure
}

[CreateAssetMenu(menuName = "Items/Item")] 
public class InventoryItem : ScriptableObject
{
    [Header("Config")]
    public string ID;
    public string Name;
    public Sprite Icon;
    public string ItemID;
    [TextArea] public string Description;

    [Header("Info")]
    public ItemType Type;
    public bool IsConsumable;
    public bool IsStackable;
    public int MaxStack;

    [HideInInspector] public int Quantity;

    private void OnValidate()
    {
#if UNITY_EDITOR//只在编译器里运行
        string path = AssetDatabase.GetAssetPath(this);
        ItemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public InventoryItem CopyItem()
    {
        InventoryItem instance = Instantiate(this);
        return instance;
    }

    public virtual bool UseItem()
    {
        return true;
    }

    public virtual void EquipItem()
    {

    }

    public virtual void RemoveItem()
    {

    }

}

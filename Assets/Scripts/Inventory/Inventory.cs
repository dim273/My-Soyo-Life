using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [Header("Config")]
    [SerializeField] private int inventorySize;
    [SerializeField] private InventoryItem[] inventoryItems;

    [Header("Testing")]
    public InventoryItem testItem;

    public int InventorySize => inventorySize;

    public void Start()
    {
        inventoryItems = new InventoryItem[inventorySize];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            AddItem(testItem, 25);
        }
    }

    private void AddItem(InventoryItem item, int quantity)
    {
        if (item == null || quantity <= 0) return;
        List<int> itemIndexs = CheckItemStack(item.ID);
        if (item.IsStackable && itemIndexs.Count > 0) 
        {
            foreach (int index in itemIndexs) 
            {
                int maxStack = item.MaxStack;
                if (inventoryItems[index].Quantity < maxStack) 
                {
                    inventoryItems[index].Quantity += quantity;
                    if (inventoryItems[index].Quantity > maxStack)
                    {
                        int dif = inventoryItems[index].Quantity - maxStack;
                        inventoryItems[index].Quantity = maxStack; 
                        AddItem(item, dif);
                    }
                    InventoryUI.instance.DrawItem(inventoryItems[index], index);
                    return;
                }
            }
            int quantityToAdd = quantity > item.MaxStack ? item.MaxStack :quantity;
            AddItemFreeSlot(item, quantityToAdd);
            int remainingAmount = quantity - quantityToAdd;
            if (remainingAmount > 0)
            {
                AddItem(item, remainingAmount);
            }

        }
    }

    private void AddItemFreeSlot(InventoryItem item, int quantity)
    {
        // 寻找空位置添加物品
        for (int i = 0; i < inventorySize; i++) 
        {
            if (inventoryItems[i] != null) continue;
            inventoryItems[i] = item.CopyItem();
            inventoryItems[i].Quantity = quantity;
            InventoryUI.instance.DrawItem(inventoryItems[i], i);
            return; 
        }
    }

    private List<int> CheckItemStack(string itemID)
    {
        // 查找指定物品当前库中是否存在有
        List<int> result = new List<int>();
        for (int i = 0; i < inventoryItems.Length; i++) 
        {
            if (inventoryItems[i] == null) continue;
            if (inventoryItems[i].ID == itemID)
            {
                result.Add(i);
            }
        }
        return result;
    }
}

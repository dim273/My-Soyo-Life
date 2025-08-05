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
        VerifyItemForDraw();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            AddItem(testItem, 2);
        }
    }

    private void AddItem(InventoryItem item, int quantity)
    {
        // 首先判断防止出错
        if (item == null || quantity <= 0) return;

        // 紧接着检查背包里是否有这件物品，如果有则
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
        }
        // 如果没有，则增加空格
        int quantityToAdd = quantity > item.MaxStack ? item.MaxStack : quantity;
        AddItemFreeSlot(item, quantityToAdd);
        int remainingAmount = quantity - quantityToAdd;
        if (remainingAmount > 0)
        {
            AddItem(item, remainingAmount);
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


    public void UseItem(int index)
    {
        // 使用物品
        if (inventoryItems[index] == null) return;
        if (inventoryItems[index].UseItem())
        {
            DecreaseItemStack(index);
        }
    }

    private void DecreaseItemStack(int index)
    {
        // 减少物品的数量，然后分两种情况再讨论
        inventoryItems[index].Quantity--;
        if (inventoryItems[index].Quantity <= 0)
        {
            inventoryItems[index] = null;
            InventoryUI.instance.DrawItem(null, index);
        }
        else
        {
            InventoryUI.instance.DrawItem(inventoryItems[index], index);
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

    private void VerifyItemForDraw()
    {
        for(int i = 0; i < inventorySize; i++)
        {
            if (inventoryItems[i] == null)
            {
                InventoryUI.instance.DrawItem(null, i);
            }
        }
    }
}

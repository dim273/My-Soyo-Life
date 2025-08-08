using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [Header("Config")]
    [SerializeField] private GameContent gameContent;
    [SerializeField] private int inventorySize;
    [SerializeField] private InventoryItem[] inventoryItems;

    [Header("Testing")]
    public InventoryItem testItem;

    public int InventorySize => inventorySize;
    public InventoryItem[] InventoryItems => inventoryItems;

    private readonly string INVENTORY_KEY_DATA = "MY_INVENTORY";

    public void Start()
    {
        inventoryItems = new InventoryItem[inventorySize];
        VerifyItemForDraw();
        LoadInventory();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            AddItem(testItem, 1);
        }
    }

    private void AddItem(InventoryItem item, int quantity)
    {
        // �����жϷ�ֹ����
        if (item == null || quantity <= 0) return;

        // �����ż�鱳�����Ƿ��������Ʒ���������
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
                    SaveInventory();
                    return;
                }
            }
        }
        // ���û�У������ӿո�
        int quantityToAdd = quantity > item.MaxStack ? item.MaxStack : quantity;
        AddItemFreeSlot(item, quantityToAdd);
        int remainingAmount = quantity - quantityToAdd;
        if (remainingAmount > 0)
        {
            AddItem(item, remainingAmount);
        }
        SaveInventory();
    }

    private void AddItemFreeSlot(InventoryItem item, int quantity)
    {
        // Ѱ�ҿ�λ�������Ʒ
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
        // ʹ����Ʒ
        if (inventoryItems[index] == null) return;

        // ��Ϊ��������װ��
        if (inventoryItems[index].Type == ItemType.Weapon)
        {
            inventoryItems[index].EquipItem();
            return;
        }

        if (inventoryItems[index].UseItem())
        {
            DecreaseItemStack(index);
            SaveInventory();
        }
    }

    public void RemoveItem(int index)
    {
        if (inventoryItems[index] == null) return;
        inventoryItems[index].RemoveItem();
        inventoryItems[index] = null;
        InventoryUI.instance.DrawItem(null, index);
        SaveInventory();
    }

    private void DecreaseItemStack(int index)
    {
        // ������Ʒ��������Ȼ����������������
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
        // ����ָ����Ʒ��ǰ�����Ƿ������
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

    private InventoryItem ItemExistsInGameContent(string itemID)
    {
        // �����е���Ϸ��Ʒ����Ѱ�Ҷ�Ӧ����Ʒ
        for (int i = 0; i < gameContent.GameItems.Length; i++)
        {
            if (gameContent.GameItems[i].ID == itemID)
            {
                return gameContent.GameItems[i];
            }
        }
        return null;
    }

    private void LoadInventory()
    {
        // ���ر������Ʒ
        if (SaveGame.Exists(INVENTORY_KEY_DATA))
        {
            InventoryData loadData = SaveGame.Load<InventoryData>(INVENTORY_KEY_DATA);
            for(int i = 0;i < inventorySize; i++)
            {
                if (loadData.ItemContent[i] != null)
                {
                    InventoryItem itemFromContent = ItemExistsInGameContent(loadData.ItemContent[i]);
                    if (itemFromContent != null)
                    {
                        inventoryItems[i] = itemFromContent.CopyItem();
                        inventoryItems[i].Quantity = loadData.ItemQuantity[i];
                        InventoryUI.instance.DrawItem(inventoryItems[i], i);
                    }
                }
                else
                {
                    inventoryItems[i] = null;
                }
            }
        }
    }

    private void SaveInventory()
    {
        // ���汳������
        InventoryData saveData = new InventoryData();
        saveData.ItemContent = new string[inventorySize];
        saveData.ItemQuantity = new int[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventoryItems[i] == null)
            {
                saveData.ItemContent[i] = null;
                saveData.ItemQuantity[i] = 0;
            }
            else
            {
                saveData.ItemContent[i] = inventoryItems[i].ID;
                saveData.ItemQuantity[i] = inventoryItems[i].Quantity;
            }
        }
        SaveGame.Save(INVENTORY_KEY_DATA, saveData);
    }
}

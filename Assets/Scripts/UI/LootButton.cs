using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LootButton : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemQuantity;

    public DropItem ItemLoaded { get; private set; }

    public void ConfigLootButton(DropItem dropItem)
    {
        // 初始化配置
        ItemLoaded = dropItem;
        itemIcon.sprite = dropItem.Item.Icon;
        itemName.text = dropItem.Item.Name;
        itemQuantity.text = $"x{dropItem.Quantity.ToString()}";
    }

    public void CollectItem()
    {
        // 拾取功能
        if (ItemLoaded == null) return;
        Inventory.instance.AddItem(ItemLoaded.Item, ItemLoaded.Quantity);
        ItemLoaded.PickedItem = true;
        Destroy(gameObject);
    }
}

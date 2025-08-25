using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCost;
    [SerializeField] private TextMeshProUGUI buyAmount;
    [SerializeField] private TextMeshProUGUI totalCost;

    private ShopItem item;
    private int quantity;
    private int initialCost;
    private int currentCost;

    private void Update()
    {
        buyAmount.text = quantity.ToString();
        totalCost.text = currentCost.ToString();
    }

    public void ConfigShopCard(ShopItem shopItem)
    {
        item = shopItem;
        itemIcon.sprite = shopItem.Item.Icon;
        itemName.text = shopItem.Item.Name;
        quantity = 0;
        initialCost = shopItem.ItemCost;
        currentCost = 0;
        buyAmount.text = "0";
        totalCost.text = "0";
        itemCost.text = initialCost.ToString();
    }

    public void Add()
    {
        // 增加按钮
        float buyCost = initialCost * (quantity + 1);
        if (CoinManager.instance.Coins >= buyCost) 
        {
            quantity++;
            currentCost = initialCost * quantity;
        }
    }

    public void Remove()
    {
        // 减少按钮 
        if (quantity == 0) return;
        quantity--;
        currentCost = initialCost * quantity;
    }

    public void BuyItem()
    {
        if (CoinManager.instance.Coins >= currentCost && quantity > 0)
        {
            Inventory.instance.AddItem(item.Item, quantity);
            CoinManager.instance.RemoveCoins(currentCost);
            quantity = 0;
            currentCost = 0;
        }
    }
}

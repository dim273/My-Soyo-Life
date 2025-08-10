using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [Header("Config")]
    [SerializeField] private GameObject lootPanel;
    [SerializeField] private LootButton lootButtonPrefab;
    [SerializeField] private Transform container;

    public void ShowLoot(EnemyLoot enemyLoot)
    {
        lootPanel.SetActive(true);
        // 删除原来的内容
        if (LootPanelWithItems())
        {
            for (int i = 0; i < container.childCount; i++)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
        // 根据enemyLoot生成掉落物
        foreach (DropItem item in enemyLoot.Items)
        {
            if (item.PickedItem) continue;
            LootButton lootButton = Instantiate(lootButtonPrefab, container);
            lootButton.ConfigLootButton(item);
        }
    }

    public void ClosePanel()
    {
        // 关闭窗口
        lootPanel.SetActive(false);
    }

    private bool LootPanelWithItems()
    {
        // 检查是否存在子对象
        return container.childCount > 0;
    }

} 

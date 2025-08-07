using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Singleton<InventoryUI>
{
    [Header("Config")]
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private Transform container;

    [Header("BagPanel Menu")]
    [SerializeField] private GameObject descisionMenu;
    [SerializeField] private TextMeshProUGUI descisionTitleTMP;

    [Header("Description Panel")]
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private bool useOrDelete = true;


    public GameObject DescisionMenu => descisionMenu;

    public InventorySlot curSlot {  get; set; }

    private List<InventorySlot> slotList = new List<InventorySlot>();

    private void Start()
    {
        InitInventory();
    }

    private void Update()
    {
        if (UIManager.instance.ifBagPanelOpen())
        {
            // 根据输入判断使用还是删除物品
            if (Input.GetKeyDown(KeyCode.Q))
            {
                useOrDelete = true;
                descisionTitleTMP.text = "是否使用物品?";
                SetDesicionMenu();
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                useOrDelete = false;
                descisionTitleTMP.text = "是否删除物品?";
                SetDesicionMenu();
            }
        }
    }

    private void SetDesicionMenu()
    {
        // 控制决策菜单的开关
        descisionMenu.SetActive(!descisionMenu.activeSelf);
    }

    private void InitInventory()
    {
        // 初始化背包
        for(int i = 0; i <Inventory.instance.InventorySize; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, container);
            slot.Index = i;
            slotList.Add(slot);
        }
    }

    public void CloseInventory()
    {
        descisionMenu.SetActive(false);
        curSlot = null;
        descriptionPanel.SetActive(false);
    }

    private void UseItem()
    {
        // 使用物品
        if (curSlot == null) return;
        Inventory.instance.UseItem(curSlot.Index);
    }

    private void RemoveItem()
    {
        // 删除物品
        if (curSlot == null) return;
        Inventory.instance.RemoveItem(curSlot.Index);
    }

    public void DrawItem(InventoryItem item, int index)
    {
        // 绘制物品栏
        InventorySlot slot = slotList[index];
        if (item == null) 
        {
            slot.ShowSlotInformation(false);
            return;
        }
        slot.ShowSlotInformation(true);
        slot.UpdateSlot(item);
    }

    public void ShowItemDescription(int index)
    {
        if (Inventory.instance.InventoryItems[index] == null) return;
        descriptionPanel.SetActive(true);
        itemIcon.sprite = Inventory.instance.InventoryItems[index].Icon;
        itemIcon.SetNativeSize();
        itemName.text = Inventory.instance.InventoryItems[index].Name;
        itemDescription.text = Inventory.instance.InventoryItems[index].Description;
    }

    public void YesButton()
    {
        if (useOrDelete) UseItem();
        else RemoveItem();
        SetDesicionMenu();
    }

    public void NoButton()
    {
        SetDesicionMenu();
    }

    private void SlotSelectedCallback(int index)
    {
        curSlot = slotList[index];
        ShowItemDescription(index);
    }

    private void OnEnable()
    {
        InventorySlot.OnSlotSelectedEvent += SlotSelectedCallback;
    }

    private void OnDisable()
    {
        InventorySlot.OnSlotSelectedEvent -= SlotSelectedCallback;
    }
}

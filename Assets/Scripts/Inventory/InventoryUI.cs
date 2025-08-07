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
            // ���������ж�ʹ�û���ɾ����Ʒ
            if (Input.GetKeyDown(KeyCode.Q))
            {
                useOrDelete = true;
                descisionTitleTMP.text = "�Ƿ�ʹ����Ʒ?";
                SetDesicionMenu();
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                useOrDelete = false;
                descisionTitleTMP.text = "�Ƿ�ɾ����Ʒ?";
                SetDesicionMenu();
            }
        }
    }

    private void SetDesicionMenu()
    {
        // ���ƾ��߲˵��Ŀ���
        descisionMenu.SetActive(!descisionMenu.activeSelf);
    }

    private void InitInventory()
    {
        // ��ʼ������
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
        // ʹ����Ʒ
        if (curSlot == null) return;
        Inventory.instance.UseItem(curSlot.Index);
    }

    private void RemoveItem()
    {
        // ɾ����Ʒ
        if (curSlot == null) return;
        Inventory.instance.RemoveItem(curSlot.Index);
    }

    public void DrawItem(InventoryItem item, int index)
    {
        // ������Ʒ��
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

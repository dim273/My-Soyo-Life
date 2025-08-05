using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    [Header("Config")]
    [SerializeField] private GameObject descisionMenu;
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private Transform container;

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
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SetDesicionMenu();
            }
        }
    }

    public void SetDesicionMenu()
    {
        descisionMenu.SetActive(!descisionMenu.activeSelf);
    }

    private void InitInventory()
    {
        for(int i = 0; i <Inventory.instance.InventorySize; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, container);
            slot.Index = i;
            slotList.Add(slot);
        }
    }

    private void UseItem()
    {
        Inventory.instance.UseItem(curSlot.Index);
    }

    public void DrawItem(InventoryItem item, int index)
    {
        InventorySlot slot = slotList[index];
        if (item == null) 
        {
            slot.ShowSlotInformation(false);
            return;
        }
        slot.ShowSlotInformation(true);
        slot.UpdateSlot(item);
    }

    public void YesButton()
    {
        UseItem();
    }

    public void NoButton()
    {
        SetDesicionMenu();
    }

    private void SlotSelectedCallback(int index)
    {
        curSlot = slotList[index];
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

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DG.Tweening;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public int maxStackeditem = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    [Header("Animations")]
    public RectTransform ToolbarRect;
    public RectTransform MainInventorRect;
    public CanvasGroup DarkBackground;
    [SerializeField] public float tweenDuration = 0.5f;

    public static InventoryManager GetInstance() { return me; }
    public static InventoryManager me;

    int selectedSlot = -1;

    void Start()
    {
        ChangeSelectedSlot(0);
    }

    void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);

            if (isNumber && number > 0 && number < 4)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
            inventorySlots[selectedSlot].Deselect();

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    void Awake()
    {
        if (me != null)
        {
            Destroy(gameObject);
            return;
        }

        me = this;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null &&
                itemInSlot.item.type == item.type &&
                ((item.type == ItemType.Bug && itemInSlot.item.bugData.id == item.bugData.id) ||
                 (item.type == ItemType.Seed && itemInSlot.item.seedData.id == item.seedData.id)) &&
                itemInSlot.count < itemInSlot.item.maxStack &&
                itemInSlot.item.Stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitaliseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;

            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                    Destroy(itemInSlot.gameObject);
                else
                    itemInSlot.RefreshCount();
            }

            return item;
        }

        return null;
    }

    public void RemoveItem(Item item, bool removeOne = false)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.type == item.type && itemInSlot.item.bugData.id == item.bugData.id)
            {
                if (removeOne)
                {
                    itemInSlot.count--;
                    if (itemInSlot.count <= 0)
                        Destroy(itemInSlot.gameObject);
                    else
                        itemInSlot.RefreshCount();
                }
                else
                {
                    Destroy(itemInSlot.gameObject);
                }
                return;
            }
        }
    }

    public void InventoryIntro()
    {
        AudioManager.GetInstance().PlayPanelEffect(true);

        DarkBackground.alpha = 0;
        DarkBackground.DOFade(1, tweenDuration).SetUpdate(true);

        MainInventorRect.anchoredPosition = new Vector2(0, 430);
        MainInventorRect.DOKill();
        MainInventorRect.DOAnchorPosY(0, tweenDuration).SetUpdate(true);
    }

    public async Task InventoryOutro()
    {
        AudioManager.GetInstance().PlayPanelEffect(false);

        DarkBackground.DOFade(0, tweenDuration).SetUpdate(true);
        MainInventorRect.DOKill();
        await MainInventorRect.DOAnchorPosY(430, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    public void ShowToolbar()
    {
        ToolbarRect.DOKill();
        ToolbarRect.DOAnchorPosY(0, tweenDuration).SetUpdate(true);
    }

    public void HideToolbar()
    {
        ToolbarRect.DOKill();
        ToolbarRect.DOAnchorPosY(-75, tweenDuration).SetUpdate(true);
    }

    public ObservableCollection<Item> GetAllBugs()
    {
        ObservableCollection<Item> bugs = new ObservableCollection<Item>();

        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.type == ItemType.Bug)
            {
                for (int i = 0; i < itemInSlot.count; i++)
                {
                    bugs.Add(itemInSlot.item);
                }
            }
        }

        return bugs;
    }
}
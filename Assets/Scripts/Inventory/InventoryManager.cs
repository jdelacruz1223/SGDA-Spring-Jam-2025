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

    [SerializeField] private bool isInventoryOpen = false;
    private bool isAnimating = false;

    public static InventoryManager GetInstance() { return me; }
    public static InventoryManager me;

    int selectedSlot = -1;

    void Start()
    {
        ChangeSelectedSlot(0);

        if (MainInventorRect != null)
        {
            MainInventorRect.gameObject.SetActive(false);
        }

        if (DarkBackground != null)
        {
            DarkBackground.alpha = 0;
        }
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

    public async Task InventoryIntro()
    {
        if (isInventoryOpen || isAnimating) return;

        isAnimating = true;

        DarkBackground.gameObject.SetActive(true);
        MainInventorRect.gameObject.SetActive(true);

        MainInventorRect.anchoredPosition = new Vector2(0, 430);
        DarkBackground.alpha = 0;

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        sequence.Append(DarkBackground.DOFade(1, tweenDuration * 0.5f));
        sequence.Append(MainInventorRect.DOAnchorPosY(0, tweenDuration))
            .OnStart(() => AudioManager.GetInstance().PlayPanelEffect(true))
            .OnComplete(() =>
            {
                isInventoryOpen = true;
                isAnimating = false;
                tcs.SetResult(true);
            });

        await tcs.Task;
    }

    public async Task InventoryOutro()
    {
        if (!isInventoryOpen || isAnimating) return;

        isAnimating = true;

        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        sequence.Append(MainInventorRect.DOAnchorPosY(430, tweenDuration))
            .OnStart(() => AudioManager.GetInstance().PlayPanelEffect(false));
        sequence.Append(DarkBackground.DOFade(0, tweenDuration * 0.5f))
            .OnComplete(() =>
            {
                MainInventorRect.gameObject.SetActive(false);
                DarkBackground.gameObject.SetActive(false);
                isInventoryOpen = false;
                isAnimating = false;
                tcs.SetResult(true);
            });

        await tcs.Task;
    }

    public void ShowToolbar()
    {
        if (ToolbarRect == null) return;

        ToolbarRect.DOKill();
        ToolbarRect.DOAnchorPosY(0, tweenDuration).SetUpdate(true);
    }

    public void HideToolbar()
    {
        if (ToolbarRect == null) return;

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

    public bool IsInventoryOpen()
    {
        return isInventoryOpen;
    }
}
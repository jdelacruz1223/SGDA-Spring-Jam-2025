using System.Threading.Tasks;
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

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < itemInSlot.item.maxStack && itemInSlot.item.Stackable)
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


    public void InventoryIntro()
    {

    }

    public void InventoryOutro()
    {

    }

    public void ShowToolbar()
    {
        ToolbarRect.DOAnchorPosY(0, tweenDuration).SetUpdate(true);
    }

    public void HideToolbar()
    {
        ToolbarRect.DOAnchorPosY(-75, tweenDuration).SetUpdate(true);
    }
}
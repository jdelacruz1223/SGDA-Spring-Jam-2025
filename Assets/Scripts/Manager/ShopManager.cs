using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    public GameObject BuyContent;
    public GameObject SellContent;
    public GameObject ItemTemplate;

    public TextMeshProUGUI currencyTxt;

    [SerializeField] public RectTransform ShopPanelRect;
    [SerializeField] public float tweenDuration = 0.3f;

    [SerializeField] private bool isOpen = false;
    private bool isAnimating = false;

    private Item[] sortedSeeds;
    private Item[] sortedBugs;

    public static ShopManager GetInstance() { return me; }
    public static ShopManager me;
    void Awake()
    {
        if (me != null)
        {
            Destroy(gameObject);
            return;
        }

        me = this;

        // Ensure shop is hidden on start
        if (ShopPanelRect != null)
        {
            ShopPanelRect.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        SeedModel[] seeds = JSONManager.GetInstance().GetSeedTypes();

        sortedSeeds = seeds.Select(seed =>
        {
            Item item = ScriptableObject.CreateInstance<Item>();
            item.type = ItemType.Seed;
            item.image = Resources.Load<Sprite>("Seeds/" + seed.id);
            item.seedData = seed;
            return item;
        }).OrderBy(s => s.seedData.price).ToArray();

        foreach (Item seed in sortedSeeds)
        {
            GameObject item = Instantiate(ItemTemplate, BuyContent.transform);
            Transform bttn = item.transform.Find("Button");

            item.SetActive(true);
            item.transform.Find("Image").GetComponent<Image>().sprite = seed.image;

            bttn.GetComponentInChildren<TextMeshProUGUI>().text = "Buy for $" + seed.seedData.price;
            bttn.GetComponent<Button>().onClick.AddListener(() =>
            {
                int index = item.transform.GetSiblingIndex();
                if (index == 0) BuySeed(0); else BuySeed(index - 1);
            });
        }

        UpdateShopUI();
    }

    public void PopulateBugList(ObservableCollection<Item> bugs)
    {
        foreach (Transform child in SellContent.transform)
        {
            Destroy(child.gameObject);
        }

        sortedBugs = bugs.OrderBy(bug => bug.bugData.price).ToArray();

        foreach (Item bug in sortedBugs)
        {
            GameObject item = Instantiate(ItemTemplate, SellContent.transform);
            Transform bttn = item.transform.Find("Button");
            item.SetActive(true);
            item.transform.Find("Image").GetComponent<Image>().sprite = bug.image;

            bttn.GetComponentInChildren<TextMeshProUGUI>().text = "Sell for $" + bug.bugData.price;
            bttn.GetComponent<Button>().onClick.AddListener(() =>
            {
                int index = item.transform.GetSiblingIndex();
                if (index == 0) SellBug(0); else SellBug(index);
            });
        }
    }

    public void SellBug(int index)
    {
        AudioManager.GetInstance().PlaySellSound();
        GameDataManager.GetInstance().AddCurrency(sortedBugs[index].bugData.price);
        InventoryManager.GetInstance().RemoveItem(sortedBugs[index], true);
        UpdateShopUI();

        ObservableCollection<Item> bugs = InventoryManager.GetInstance().GetAllBugs();
        PopulateBugList(bugs);
    }

    public void BuySeed(int index)
    {
        AudioManager.GetInstance().PlayBuySound();
        if (GameDataManager.GetInstance().SpendCurrency(sortedSeeds[index].seedData.price)) InventoryManager.GetInstance().AddItem(sortedSeeds[index]);
        else Debug.Log("Can't Afford! You only have " + GameDataManager.GetInstance().playerCurrency);

        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        currencyTxt.text = GameDataManager.GetInstance().playerCurrency.ToString();
    }

    public async Task ShopIntro()
    {
        if (isOpen || isAnimating) return;

        isAnimating = true;
        ObservableCollection<Item> bugs = InventoryManager.GetInstance().GetAllBugs();
        PopulateBugList(bugs);

        InventoryManager.GetInstance().HideToolbar();
        ShopPanelRect.gameObject.SetActive(true);

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) player.DisablePlayerMovement();

        ShopPanelRect.anchoredPosition = new Vector2(ShopPanelRect.anchoredPosition.x, 430);

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        ShopPanelRect.DOKill();
        ShopPanelRect.DOAnchorPosY(0, tweenDuration)
            .SetUpdate(true)
            .OnStart(() => AudioManager.GetInstance().PlayPanelEffect(true))
            .OnComplete(() =>
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                isOpen = true;
                isAnimating = false;
                tcs.SetResult(true);
            });

        await tcs.Task;
    }

    public async Task ShopOutro()
    {
        if (!isOpen || isAnimating) return;

        isAnimating = true;
        InventoryManager.GetInstance().ShowToolbar();

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        ShopPanelRect.DOKill();
        ShopPanelRect.DOAnchorPosY(430, tweenDuration)
            .SetUpdate(true)
            .OnStart(() => AudioManager.GetInstance().PlayPanelEffect(false))
            .OnComplete(() =>
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ShopPanelRect.gameObject.SetActive(false);
                isOpen = false;
                isAnimating = false;
                if (!InventoryManager.GetInstance().IsInventoryOpen())
                {
                    PlayerController player = FindFirstObjectByType<PlayerController>();
                    if (player != null) player.EnablePlayerMovement();
                }
                tcs.SetResult(true);
            });

        await tcs.Task;
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public void CloseShopButton()
    {
        if (isOpen && !isAnimating)
        {
            StartCoroutine(CloseShopCoroutine());
        }
    }

    private IEnumerator CloseShopCoroutine()
    {
        var operation = ShopOutro();
        while (!operation.IsCompleted)
        {
            yield return null;
        }
    }
}

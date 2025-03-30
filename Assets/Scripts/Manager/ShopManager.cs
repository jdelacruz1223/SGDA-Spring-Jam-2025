using System;
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
    [SerializeField] public float tweenDuration;

    private Item[] sortedSeeds;

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
    }

    void Start()
    {
        SeedModel[] seeds = JSONManager.GetInstance().GetSeedTypes();

        sortedSeeds = seeds.Select(seed =>
        {
            Item item = ScriptableObject.CreateInstance<Item>();
            item.type = ItemType.Seed;
            item.image = Resources.Load<Sprite>("Seeds/" + seed.plantId);
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

    public void BuySeed(int index)
    {
        if (GameDataManager.GetInstance().SpendCurrency(sortedSeeds[index].seedData.price)) InventoryManager.GetInstance().AddItem(sortedSeeds[index]);
        else Debug.Log("Can't Afford! You only have " + GameDataManager.GetInstance().playerCurrency);

        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        currencyTxt.text = GameDataManager.GetInstance().playerCurrency + " coins";
    }

    public void ShopIntro()
    {
        InventoryManager.GetInstance().HideToolbar();
        ShopPanelRect.DOKill();
        ShopPanelRect.DOAnchorPosY(0, tweenDuration).SetUpdate(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        ShopPanelRect.gameObject.SetActive(true);
    }

    async public void ShopOutro()
    {

        InventoryManager.GetInstance().ShowToolbar();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ShopPanelRect.DOKill();
        await Task.Run(async () => await ShopPanelRect.DOAnchorPosY(430, tweenDuration).SetUpdate(true).AsyncWaitForCompletion());
        ShopPanelRect.gameObject.SetActive(false);
    }
}

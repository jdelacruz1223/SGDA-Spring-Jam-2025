using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

    private Item[] sortedSeeds;
    void Start()
    {

        Item[] seeds = Resources.LoadAll<Item>("Seeds");

        sortedSeeds = seeds.Where(s => s.seedData != null).OrderBy(s => s.seedData.price).ToArray();

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
    }

    public void BuySeed(int index)
    {
        if (GameDataManager.GetInstance().SpendCurrency(sortedSeeds[index].seedData.price)) InventoryManager.GetInstance().AddItem(sortedSeeds[index]);
        else Debug.Log("Can't Afford! You only have " + GameDataManager.GetInstance().playerCurrency);
    }

    void Update()
    {

    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    public GameObject BuyContent;
    public GameObject SellContent;
    public GameObject ItemTemplate;


    void Start()
    {

        Seeds[] seeds = Resources.LoadAll<Seeds>("Seeds");

        foreach (Seeds seed in seeds)
        {

            GameObject item = Instantiate(ItemTemplate, BuyContent.transform);
            item.GetComponentInChildren<TextMeshProUGUI>().text = seed.seedData.name;

            item.SetActive(true);
        }
    }

    void Update()
    {

    }
}

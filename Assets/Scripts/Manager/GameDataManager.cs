using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

/// <summary>
/// Manages all game data including player currency, inventory, plants, and bugs.
/// </summary>
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager GetInstance() { return me; }
    public static GameDataManager me;

    [Header("Player Resources")]
    public int playerCurrency = 100;

    [Header("Progression")]
    public int gardenLevel = 1;
    public ObservableCollection<BugModel> discoveredBugTypes = new ObservableCollection<BugModel>();
    public ObservableCollection<SeedModel> discoveredSeedTypes = new ObservableCollection<SeedModel>();

    [Header("Statistics")]
    public int totalBugsCaught;
    public int totalPlantsGrown;
    public int totalMoneyEarned;
    public int totalMoneySpent;
    public BugModel currentBug;

    void Awake()
    {
        if (me != null)
        {
            Destroy(gameObject);
            return;
        }

        me = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddBug(string id)
    {
        BugModel bug = JSONManager.GetInstance().GetBugById(id);

        if (!discoveredBugTypes.Contains(bug))
            discoveredBugTypes.Add(bug);

        Item item = ScriptableObject.CreateInstance<Item>();

        item.bugData = bug;
        item.image = Resources.Load<Sprite>("Bugs/" + bug.id);
        item.Stackable = bug.stackable;
        item.type = ItemType.Bug;

        InventoryManager.GetInstance().AddItem(item);
        totalBugsCaught++;
    }

    public bool CanAfford(int amount)
    {
        return playerCurrency >= amount;
    }

    public void AddCurrency(int amount)
    {
        playerCurrency += amount;
        totalMoneyEarned += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (!CanAfford(amount)) return false;
        playerCurrency -= amount;
        totalMoneySpent += amount;
        return true;
    }

    #region Plant Functions
    // public void IncrementBugCount(PlantData plantType) {

    // }
    #endregion
}
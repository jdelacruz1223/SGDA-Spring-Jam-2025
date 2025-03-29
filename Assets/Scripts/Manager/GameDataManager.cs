using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

    void Awake()
    {
        if (me != null)
        {
            Destroy(gameObject);
            return;
        }

        me = this;
        DontDestroyOnLoad(gameObject);
        InitializeGameData();
    }

    private void InitializeGameData()
    {

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
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int money;
    public string[] seedInventory = {"FernSeed", "SeedTwo"}; // [FernSeed, ...]; expandable
    private int currentSeedIndex;
    private Vector3 currentPos;
    public Bug[] bugInventory;
    public Dictionary<string, int> currentSeed;

    private void TakeBug(Plant plant) {

    }

    private void PlantSeed(int currentSeedIndex) {

    }

}

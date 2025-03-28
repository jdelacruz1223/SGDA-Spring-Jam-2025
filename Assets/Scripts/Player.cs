using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int money;
    public Dictionary<string, int> SeedInventory = new Dictionary<string, int> {
        { "Fern_Seed", 0} 
    }; // expandable
    private Vector3 currentPos;
    public Bug[] bugInventory;
    public Dictionary<string, int> currentSeed;

    private void TakeBug(Plant plant) {

    }

    private void PlantSeed(Dictionary<string, int> currentSeed) {

    }

}

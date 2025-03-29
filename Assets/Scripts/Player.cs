using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int money;
    [SerializeField] public string[] seedInventory = {"FernSeed", "SeedTwo"}; // [FernSeed, ...]; expandable
    private int currentSeedIndex;
    private Vector3 currentPos;
    public Bug[] bugInventory;
    public Dictionary<string, int> currentSeed;
    [SerializeField] public GameObject plantPrefab;
    [SerializeField] public float plantingOffset;

    private void TakeBug(Plant plant) {

    }

    private void PlantSeed(int currentSeedIndex) {
        Instantiate(plantPrefab, transform.position + transform.forward * plantingOffset, Quaternion.identity);
    }

}

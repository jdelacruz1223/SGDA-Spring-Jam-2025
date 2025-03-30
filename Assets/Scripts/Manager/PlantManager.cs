using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public List<Seed> allSeeds = new List<Seed>();
    public float checkInterval = 1f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            CheckPlants();
            timer = 0f;
        }
    }

    void CheckPlants()
    {
        foreach (var seed in allSeeds.ToList())
        {
            if (seed.CheckIfGrown())
            {
                seed.Grow();
                allSeeds.Remove(seed);
            }
        }
    }

    public void RegisterSeed(Seed seed)
    {
        allSeeds.Add(seed);
    }
}

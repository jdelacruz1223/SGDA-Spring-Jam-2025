using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    public GameObject seedPrefab;

    public void PlantSeed(SeedModel seedModel, Vector3 position)
    {
        GameObject newPlant = Instantiate(seedPrefab, position, Quaternion.identity);

        Debug.Log("Planting seed: " + seedModel.name);
        Seed seed = newPlant.GetComponent<Seed>();
        
        seed.Initalize(seedModel);

        FindFirstObjectByType<PlantManager>().RegisterSeed(seed);
    }
}

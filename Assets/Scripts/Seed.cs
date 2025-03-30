using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public GameObject placeHolder;
    [SerializeField] public TextMeshPro timerTxt;

    public PlantModel plantData;
    public SeedModel seedData;
    private DateTime plantedTime;
    private bool hasGrown = false;

    private void Update()
    {
        if (timerTxt != null)
        {
            timerTxt.transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void Initalize(SeedModel seed)
    {
        seedData = seed;
        plantData = JSONManager.GetInstance().GetPlantById(seed.plantId);
        plantedTime = DateTime.Now;
    }

    public bool CheckIfGrown()
    {
        if (hasGrown) return true;

        TimeSpan elapsed = DateTime.Now - plantedTime;
        UpdateTimerText(elapsed);

        if (elapsed.TotalSeconds >= seedData.growthTime)
        {
            hasGrown = true;
            return true;
        }

        return false;
    }

    private void UpdateTimerText(TimeSpan elapsed)
    {
        TimeSpan remaining = TimeSpan.FromSeconds(seedData.growthTime) - elapsed;
        timerTxt.text = remaining.ToString(@"mm\:ss");
    }

    public void Grow()
    {
        Debug.Log($"{plantData.name} has grown!");
        timerTxt.text = plantData.name + " has grown!";
        StartCoroutine(GrowCoroutine());
    }

    private IEnumerator GrowCoroutine()
    {
        yield return new WaitForSeconds(2f);
        GameObject plantPrefab = Resources.Load<GameObject>($"Plant");

        if (plantPrefab != null)
        {
            GameObject plant = Instantiate(plantPrefab, transform.position, Quaternion.identity);
            plant.GetComponent<Plant>().InitializePlant(plantData);

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError($"Failed to load plant prefab: {plantData.prefab}");
        }
    }
}
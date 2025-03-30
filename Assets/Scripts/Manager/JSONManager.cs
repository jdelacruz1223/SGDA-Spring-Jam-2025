using System.Collections.Generic;
using UnityEngine;

public class JSONManager : MonoBehaviour
{
    public PlantDatabase plantDatabase;
    public PlantData plantData;
    public BugData bugData;
    public SeedData seedData;
    

    void Awake()
    {
        LoadData();
    }

    void Start()
    {
        Debug.Log(GetPlantTypes().ToString());
        
        if (plantDatabase == null) {
            Debug.LogError("plantDatabase is null after LoadData(). Check JSON structure.");
        return; 
        }
    }

    public void LoadData() {
        TextAsset jsonFile = Resources.Load<TextAsset>("gameData");
        if (jsonFile != null) {
            plantDatabase = JsonUtility.FromJson<PlantDatabase>(jsonFile.text);
            if (plantDatabase == null) {
                Debug.LogError("Failed to parse gameData.json into PlantDatabase.");
                return;
            }
            if (seedData == null) seedData = new SeedData();
            if (plantData == null) plantData = new PlantData();
            if (bugData == null) bugData = new BugData();

            plantData.plantTypes = plantDatabase.plants.ToArray();
            bugData.bugTypes = plantDatabase.bugs.ToArray();
            seedData.seedTypes = plantDatabase.seeds.ToArray();

        } else Debug.LogError("gameData.json not found");

        Debug.Log($"{GetPlantTypes()}, {GetSeedTypes()}, {GetBugTypes()}");
    }

    public PlantModel[] GetPlantTypes() {
        return plantDatabase.plants.ToArray();
    }
    public SeedModel[] GetSeedTypes() {
        return plantDatabase.seeds.ToArray();
    }
    public BugModel[] GetBugTypes() {
        return plantDatabase.bugs.ToArray();
    }
}

[System.Serializable]
public class PlantData {
    public PlantModel[] plantTypes;
}
public class SeedData { 
    public SeedModel[] seedTypes;
}
public class BugData {
    public BugModel[] bugTypes;
}

[System.Serializable]
public class PlantDatabase
{
    public List<SeedModel> seeds;
    public List<PlantModel> plants;
    public List<BugModel> bugs;
}



using UnityEngine;

public class JSONManager : MonoBehaviour
{
    public PlantData plantData;
    private string[] plantTypes;
    private string[] seedTypes;
    private string[] bugTypes;
    void Start()
    {
        LoadData();
    }

    public void LoadData() {
        TextAsset jsonFile = Resources.Load<TextAsset>("gameData");
        if (jsonFile != null) {
            PlantData plantData = JsonUtility.FromJson<PlantData>(jsonFile.text);
            SeedData seedData = JsonUtility.FromJson<SeedData>(jsonFile.text);
            BugData bugData = JsonUtility.FromJson<BugData>(jsonFile.text);

            plantTypes = plantData.plantTypes;
            plantTypes = seedData.seedTypes;
            plantTypes = bugData.bugTypes;

            foreach (var plant in plantTypes) {
                Debug.Log($"Loaded Plant: {plant}");
            }
            foreach (var seed in seedTypes) {
                Debug.Log($"Loaded Plant: {seed}");
            }
            foreach (var bug in bugTypes) {
                Debug.Log($"Loaded Plant: {bug}");
            }

        } else Debug.LogError("gameData.json not found");
    }

    public string[] GetPlantTypes() {
        return plantTypes;
    }
    public string[] GetSeedTypes() {
        return seedTypes;
    }
    public string[] GetBugTypes() {
        return bugTypes;
    }
}

[System.Serializable]
public class PlantData {
    public string[] plantTypes;
}
public class SeedData { // ask dichill abt this; SeedTypes
    public string[] seedTypes;
}
public class BugData {
    public string[] bugTypes;
}


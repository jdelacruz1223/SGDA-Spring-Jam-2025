using System.Collections.Generic;
using UnityEngine;

public class JSONManager : MonoBehaviour
{
    public PlantDatabase plantDatabase;
    void Start()
    {
        LoadData();
    }

    public void LoadData() {
        TextAsset jsonFile = Resources.Load<TextAsset>("gameData");
        if (jsonFile != null) {
            plantDatabase = JsonUtility.FromJson<PlantDatabase>(jsonFile.text);
        } else Debug.LogError("gameData.json not found");
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
    public string[] plantTypes;
}
public class SeedData { 
    public string[] seedTypes;
}
public class BugData {
    public string[] bugTypes;
}

[System.Serializable]
public class PlantDatabase
{
    public List<SeedModel> seeds;
    public List<PlantModel> plants;
    public List<BugModel> bugs;
}



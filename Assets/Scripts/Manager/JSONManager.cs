using UnityEngine;

public class JSONManager : MonoBehaviour
{
    public PlantData plantData;
    private string[] plantTypes;
    void Start()
    {
        LoadPlantData();
    }

    void Update()
    {
        
    }
#region Plant JSON
    public void LoadPlantData() {
        TextAsset jsonFile = Resources.Load<TextAsset>("plantData");
        if (jsonFile != null) {
            PlantData data = JsonUtility.FromJson<PlantData>("jsonFile.text");
            plantTypes = data.plantTypes;

            foreach (var plant in plantTypes) {
                Debug.Log($"Loaded Plant: {plant}");
            }
        } else Debug.LogError("plantData.json not found");
    }

#endregion
}

[System.Serializable]
public class PlantData {
    public string[] plantTypes;
}


using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Bug : MonoBehaviour
{
    //JSON
    private JSONManager jsonManager; // getter
    //JSONManager
    private PlantModel[] plantTypes;
    private SeedModel[] seedTypes;
    private BugModel[] bugTypes;
    private PlantModel currentPlantType;
    private SeedModel currentSeedType;
    private BugModel currentBugType;

    void Awake() {
        jsonManager = FindFirstObjectByType<JSONManager>();
        if (jsonManager == null) {
            Debug.LogError("JSONManager not found.");
        }
    }

    void Start() {
        plantTypes = jsonManager.GetPlantTypes();
        seedTypes = jsonManager.GetSeedTypes();
        bugTypes = jsonManager.GetBugTypes();
        currentPlantType = plantTypes[0];
        currentSeedType = seedTypes[0];
        currentBugType = bugTypes[0];
    }

    private void StartMinigame(BugModel currentBugType) {
        switch(currentBugType.id) {
            case "seed_sunflower":
                //insert scene change to beetle minigame
                break; 
            default:
                Debug.LogError("currentBugType id missing or does not exist.");
                break;
        }
    }


    public int GetBugValue() {
        return currentBugType.price;
    }

    public string GetBugName() {
        return currentBugType.name;
    }

    public string GetBugId() {
        return currentBugType.id;
    }

    public string GetBugDescription() {
        return currentBugType.description;
    }
}

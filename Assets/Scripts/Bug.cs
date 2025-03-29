using Unity.VisualScripting;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public enum BugType { FernBug }
    public BugType currentBugType; 
    public int value;

    public void Initialize(Plant.PlantType plantType, int bugValue) {
        switch(plantType) {
            case Plant.PlantType.Fern:
                currentBugType = BugType.FernBug;
                break;
            default: 
                currentBugType = BugType.FernBug;
                break;  
        }
        value = bugValue;
    }


    private void StartMinigame() {
        Debug.Log("Minigame hajimeru");
    }
}

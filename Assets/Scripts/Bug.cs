using Unity.VisualScripting;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public int value;
    private JSONManager jsonManager;
    private string[] bugTypes;

    void Start() {
        bugTypes = jsonManager.GetPlantTypes();
    }

    private void StartMinigame() {
        Debug.Log("Minigame hajimeru");
    }
}

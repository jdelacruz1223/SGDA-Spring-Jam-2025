using Unity.VisualScripting;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public int value;
    private JSONManager jsonManager;
    private string[] bugTypes;

    void Start() {
    }

    private void StartMinigame() {
        Debug.Log("Minigame hajimeru");
    }
}

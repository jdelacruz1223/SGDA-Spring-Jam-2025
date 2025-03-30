using Unity.VisualScripting;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public enum BugType { FernBug }
    public BugType currentBugType; 
    public int value;

    // TODO 
    public void Initialize(int bugValue) { 
        //insert bugType 
        value = bugValue;
    }


    private void StartMinigame() {
        Debug.Log("Minigame hajimeru");
    }
}

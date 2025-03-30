using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable
{
    //JSON
    private JSONManager jsonManager; // getter
    //Editor
    [SerializeField] private GameObject model; // assign in editor
    private BoxCollider boxCollider; // toggle collider for interaction
    //Plant Attributes
    private enum AgeState { Sprout, Mature }
    private AgeState currentAge; 
    private string[] plantTypes;
    private string currentPlantType;
    private string seedType { get; set;}

    // pass seedtype during plant prefab instantiation to set plant type
    
    void Start() {
        
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        StartCoroutine(BugSpawnTimer());
    }

    private void Update() {
        SetModel(); 
        
        //plant spawning debug keys
        if (Input.GetKeyDown(KeyCode.L)) {
            currentAge = AgeState.Mature;
            Debug.Log(currentAge);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            currentAge = AgeState.Sprout;
            Debug.Log(currentAge);
        }
    }

    private void SetModel() {
        if (currentAge == AgeState.Sprout) {
            model.SetActive(false);
            boxCollider.enabled = false;
            return;
        }
        model.SetActive(true);
        boxCollider.enabled = true;
    }


    
#region IInteractable
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public bool Interact(PlayerController playerController) {
        Debug.Log("Plant Interacted!");
        TakeBug();
        return true;
    }
#endregion

#region Bug Handling
    private bool hasBug = false;
    //make currentBug variable
    [SerializeField] private float bugSpawnDelay;
    /// <summary>
    /// A timer that attempts to spawn a bug if possible after a certain amount of time. Runs as long as this object exists.
    /// </summary>
    private IEnumerator BugSpawnTimer() {
        while (true) {
            yield return new WaitForSeconds(bugSpawnDelay);
            if(!hasBug) SpawnNewBug();
        }
    }
    
    //TODO
    private void TakeBug() { 
        if (hasBug) {
            Debug.Log("No bug to take :(");
            return;
        }
        // insert take bug functionality
        hasBug = false;
        Debug.Log("Bug taken");
    }

    //TODO
    private void SpawnNewBug() {
        if (!hasBug) {
            //insert spawn bug functionality
            
            hasBug = true;
            Debug.Log("Bug spawned");
            return;
        }
    }
#endregion

#region UI
// promptUI
#endregion
  
}



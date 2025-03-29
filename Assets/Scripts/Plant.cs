using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable
{
    private enum AgeState { Sprout, Mature }
    public enum PlantType { Fern } // expandable
    private AgeState currentAge; // currentAge
    public PlantType currentPlantType;
    [SerializeField] private GameObject bug; // initialize in SpawnBug when plant is matured
    private bool hasBug = false;
    [SerializeField] private GameObject model; // assign in editor
    [SerializeField] private GameObject bugPrefab;
    [SerializeField] private float bugSpawnDelay;
    private BoxCollider boxCollider;

    void Start() {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        StartCoroutine(BugSpawnTimer());
    }

#region Timer
    /// <summary>
    /// A timer that attempts to spawn a bug if possible after a certain amount of time. Runs as long as this object exists.
    /// </summary>
    private IEnumerator BugSpawnTimer() {
        while (true) {
            yield return new WaitForSeconds(bugSpawnDelay);
            if(!hasBug) SpawnNewBug();
        }
    }

#endregion

    
#region IInteractable
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public bool Interact(PlayerController playerController) {
        Debug.Log("Plant Interacted!");
        TakeBug();
        return true;
    }
#endregion

    private void SetModel() {
        if (currentAge == AgeState.Sprout) {
            model.SetActive(false);
            boxCollider.enabled = false;
            return;
        }
        model.SetActive(true);
        boxCollider.enabled = true;
    }

#region Bug Handling
    private void TakeBug() {
        if (bug == null) {
            Debug.Log("No bug to take :(");
            return;
        }
        // for dichill; pass bug reference to player bug inventory
        bug = null; // dereference bug object,
        hasBug = false;
        Debug.Log("Bug taken");
    }

    private void SpawnNewBug() {
        if (!hasBug) {
            GameObject newBugObj = Instantiate(bugPrefab, transform.position, Quaternion.identity);
            Bug newBug = newBugObj.GetComponent<Bug>();

            newBug.Initialize(currentPlantType, 1);
            bug = newBugObj;
            // Debug.Log("setting bug to " + bug.ToString());
            hasBug = true;
            Debug.Log("Bug spawned");
            return;
        }
    }
#endregion

#region UI

#endregion


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
}

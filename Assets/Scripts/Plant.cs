using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour, IInteractable
{
    private enum AgeState { Sprout, Mature }
    public enum PlantType { Fern } // expandable
    private AgeState currentAge; // currentAge
    public PlantType currentPlantType;
    [SerializeField] private GameObject bug; // initialize in SpawnBug when plant is matured
    private bool hasBug;
    [SerializeField] private GameObject model; // assign in editor
    [SerializeField] private GameObject bugPrefab;

    void Start() {
        Initlialize();
    }

    void Initlialize() {
        hasBug = false;
        // might put more here later
    }
#region IInteractable
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public bool Interact(PlayerController playerController) {
        Debug.Log("Plant Interacted!");
        return true;
    }
#endregion

    private void SetModel() {
        if (currentAge == 0) {
            model.SetActive(false);
            return;
        }
        model.SetActive(true);
    }

    private void SpawnNewBug(AgeState currentAge) {
        if (currentAge == AgeState.Mature && !hasBug) {
            GameObject newBugObj = Instantiate(bugPrefab, transform.position, Quaternion.identity);
            Bug newBug = newBugObj.GetComponent<Bug>();

            newBug.Initialize(currentPlantType, 1);
            bug = newBugObj;
            Debug.Log("setting bug to " + bug.ToString());
            hasBug = true;

            return;
        }
    }

    bool spawnedOnce = false;

    private void Update() {
        SetModel();
        if (Input.GetKeyDown(KeyCode.L)) {
            currentAge = AgeState.Mature;
            spawnedOnce = false;
            Debug.Log(currentAge);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            currentAge = AgeState.Sprout;
            spawnedOnce = false;
            Debug.Log(currentAge);
        }
    }
}

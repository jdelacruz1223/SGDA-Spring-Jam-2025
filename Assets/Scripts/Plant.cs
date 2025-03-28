using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
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
        if(!spawnedOnce) {
            SpawnNewBug(currentAge);
            spawnedOnce = true;
        }
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
        if (Input.GetKeyDown(KeyCode.J)) {
            if (bug != null) {
                Destroy(bug);
                bug = null;
                Debug.Log("setting bug to " + bug.ToString());
                hasBug = false;
                spawnedOnce = false;
                Debug.Log("Bug Taken");
            }
        }
    }

}

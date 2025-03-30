using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable
{
    //JSON
    private JSONManager jsonManager; // getter
    //Editor
    [SerializeField] private GameObject plantChild; // assign in editor
    [SerializeField] private GameObject bugChild; // assign in editor
    //Plant Attributes
    private enum AgeState { Sprout, Mature }
    private AgeState currentAge; 
    //JSONManager
    private PlantModel[] plantTypes;
    private SeedModel[] seedTypes;
    private BugModel[] bugTypes;
    private PlantModel currentPlantType;
    private SeedModel currentSeedType;
    private BugModel currentBugType;


    // pass seedtype during plant prefab instantiation to set plant type??
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

        //write and call function that sets this plant object's planttype
        
        currentAge = AgeState.Sprout;        
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        bugSprite = bugChild.GetComponent<SpriteRenderer>();
        plantSprite = plantChild.GetComponent<SpriteRenderer>();

        bugSprite.enabled = true;
        boxCollider.enabled = true;
        hasBug = true;

        StartCoroutine(BugSpawnTimer());
    }

    private void Update()
    {
        //plant spawning debug keys
        if (Input.GetKeyDown(KeyCode.L))
        {
            currentAge = AgeState.Mature;
            Debug.Log(currentAge);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentAge = AgeState.Sprout;
            Debug.Log(currentAge);
        }
    }

    #region IInteractable
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public bool Interact(PlayerController playerController)
    {
        Debug.Log("Plant Interacted!");
        TakeBug();
        return true;
    }
    #endregion

    #region Bug Handling
    private SpriteRenderer bugSprite;
    private SpriteRenderer plantSprite;
    private BoxCollider boxCollider; // toggle collider for interaction
    [SerializeField] private bool hasBug = false;

    
    /// <summary>
    /// A timer that attempts to spawn a bug if possible after a certain amount of time. Runs as long as this object exists.
    /// </summary>
    private IEnumerator BugSpawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSeedType.growthTime);
            if (!hasBug) SpawnNewBug();
        }
    }

    private void TakeBug()
    {
        if (!hasBug)
        {
            Debug.Log("No bug to take :(");
            return;
        }
        bugSprite.enabled = false;
        boxCollider.enabled = false;
        GameDataManager.GetInstance().bug1Count++;
        hasBug = false;
        Debug.Log("Bug taken");
    }

    private void SpawnNewBug()
    {
        if (!hasBug)
        {
            bugSprite.enabled = true;
            boxCollider.enabled = true;
            hasBug = true;
            Debug.Log("Bug enabled");
            return;
        }
    }
    #endregion

    #region UI
    // promptUI
    #endregion

}



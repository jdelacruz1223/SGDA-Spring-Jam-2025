using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject plantChild;
    [SerializeField] private GameObject bugChild;
    private PlantModel currentPlantType;
    private BugModel currentBugType;

    public void InitializePlant(PlantModel plant)
    {
        currentPlantType = plant;
        currentBugType = JSONManager.GetInstance().GetBugById(plant.bugId);

        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        bugSprite = bugChild.GetComponent<SpriteRenderer>();
        plantSprite = plantChild.GetComponent<SpriteRenderer>();


        bugSprite.sprite = Resources.Load<Sprite>("Bugs/" + currentBugType.id);
        plantSprite.sprite = Resources.Load<Sprite>("Plants/" + plant.id);

        bugSprite.enabled = true;
        boxCollider.enabled = true;

        hasBug = true;
    }

    void Start()
    {
        StartCoroutine(BugSpawnTimer());
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
            yield return new WaitForSeconds(currentBugType.spawnTime);
            Debug.Log(currentBugType.spawnTime);
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

        AudioManager.GetInstance().PlayHarvestSound();
        bugSprite.enabled = false;
        boxCollider.enabled = false;
        GameDataManager.GetInstance().AddBug(currentPlantType.bugId);


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
}
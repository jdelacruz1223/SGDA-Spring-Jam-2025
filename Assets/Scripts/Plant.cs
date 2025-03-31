using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Plant : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject plantChild;
    [SerializeField] private GameObject bugChild;
    private PlantModel currentPlantType;
    private BugModel currentBugType;

    [SerializeField] private GameObject beetleMinigamePrefab; // Reference to the BeetleMinigame prefab
    private GameObject activeMinigameInstance; // Reference to instantiated minigame object

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

        hasBug = false;

        GameDataManager.GetInstance().currentBug = currentBugType;
        GameDataManager.GetInstance().AddBug(currentPlantType.bugId);

        // StartBeetleMinigame();
    }

    private void StartBeetleMinigame()
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.DisablePlayerMovement();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (activeMinigameInstance == null)
            {
                if (beetleMinigamePrefab != null)
                {
                    activeMinigameInstance = Instantiate(beetleMinigamePrefab);
                }
                else
                {
                    GameObject prefab = Resources.Load<GameObject>("BeetleMinigame");
                    if (prefab != null)
                    {
                        activeMinigameInstance = Instantiate(prefab);
                    }
                    else
                    {
                        Debug.LogWarning("BeetleMinigame prefab not assigned or found in Resources");
                    }
                }

                BugMovement bugMovement = activeMinigameInstance.GetComponentInChildren<BugMovement>();
                if (bugMovement != null)
                {
                    StartCoroutine(CheckForMinigameCompletion());
                }
                else
                {
                    Debug.LogError("Failed to find BugMovement component in the minigame instance. The minigame may not work correctly.");
                }
            }
        }
    }

    private IEnumerator CheckForMinigameCompletion()
    {
        BugMovement bugMovement = null;
        if (activeMinigameInstance != null)
        {
            bugMovement = activeMinigameInstance.GetComponentInChildren<BugMovement>();
        }

        if (bugMovement != null)
        {
            yield return new WaitUntil(() => bugMovement.isDone);

            yield return new WaitForSeconds(2f);

            EndBeetleMinigame();
        }
        else
        {
            yield return new WaitForSeconds(5f);
            EndBeetleMinigame();
        }
    }

    private void EndBeetleMinigame()
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.EnablePlayerMovement();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (activeMinigameInstance != null)
        {
            Destroy(activeMinigameInstance);
            activeMinigameInstance = null;
        }
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
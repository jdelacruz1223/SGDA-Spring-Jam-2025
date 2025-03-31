using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

public class InteractUI : MonoBehaviour, IInteractable
{
    // [SerializeField]
    public Transform interactableUI;
    private bool isInteracting = false;

    void Start()
    {
        interactableUI = GameObject.Find("Canvas")?.transform.Find("Shop");
    }

    #region IInteractable
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(PlayerController playerController)
    {
        if (isInteracting) return false;

        isInteracting = true;

        ShopManager shopManager = ShopManager.GetInstance();

        if (shopManager.IsOpen())
        {
            StartCoroutine(HandleShopClose(shopManager, playerController));
        }
        else
        {
            StartCoroutine(HandleShopOpen(shopManager, playerController));
        }

        return true;
    }

    private IEnumerator HandleShopClose(ShopManager shopManager, PlayerController playerController)
    {
        var operation = shopManager.ShopOutro();
        while (!operation.IsCompleted)
        {
            yield return null;
        }

        playerController.EnablePlayerMovement();
        yield return ResetInteractionState();
    }

    private IEnumerator HandleShopOpen(ShopManager shopManager, PlayerController playerController)
    {
        playerController.DisablePlayerMovement();

        var operation = shopManager.ShopIntro();
        while (!operation.IsCompleted)
        {
            yield return null;
        }

        yield return ResetInteractionState();
    }

    private IEnumerator ResetInteractionState()
    {
        yield return new WaitForSeconds(0.5f);
        isInteracting = false;
    }
    #endregion
}

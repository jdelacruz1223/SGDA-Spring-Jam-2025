using System;
using UnityEngine;

public class InteractUI : MonoBehaviour, IInteractable
{
    [SerializeField]
    public Transform interactableUI;


    #region IInteractable
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public bool Interact(PlayerController playerController)
    {
        if (interactableUI.gameObject.activeSelf) AutoClose();
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            interactableUI.gameObject.SetActive(true);
        }

        return true;
    }
    #endregion

    public void AutoClose()
    {
        interactableUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

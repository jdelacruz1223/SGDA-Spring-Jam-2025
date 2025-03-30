using System;
using System.Threading.Tasks;
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
        if (interactableUI.gameObject.activeSelf)
        {
            ShopManager.GetInstance().ShopOutro();
            InventoryManager.GetInstance().ShowToolbar();
        }
        else
        {
            ShopManager.GetInstance().ShopIntro();
            InventoryManager.GetInstance().HideToolbar();
        }

        return true;
    }
    #endregion
}

using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

interface IInteractable
{
    public string InteractionPrompt { get; }
    public bool Interact(PlayerController playerController) { throw new System.NotImplementedException(); }
}
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Maximum speed player can reach
    /// </summary>
    [SerializeField] float maxSpeed;
    /// <summary>
    /// How much to multiply speed when sprinting?
    /// </summary>
    [SerializeField] float sprintMultipler;
    /// <summary>
    /// Current speed of player
    /// </summary>
    float speed;
    /// <summary>
    /// Are the movement buttons held?
    /// </summary>
    bool moveHeld = false;
    bool sprintHeld = false;
    /// <summary>
    /// The current direction of movement
    /// </summary>
    [SerializeField] Vector2 moveDir = Vector2.zero;
    /// <summary>
    /// Linear acceleration of player's speed
    /// </summary>
    [SerializeField] float acceleration;
    /// <summary>
    /// Exponnetial deceleration of player's speed
    /// </summary>
    [SerializeField] float deceleration;
    /// <summary>
    /// Refernece to character controller
    /// </summary>
    CharacterController cc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public GameObject inventoryUI;


    void Start()
    {
        if (cc == null) cc = GetComponent<CharacterController>();
        GetComponent<CameraController>().target = gameObject;
        Cursor.lockState = CursorLockMode.Locked; //lock cursor to play window
        Cursor.visible = false; //make cursor invisible
        interactableCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
        AnimatePlayer(moveDir);

    }

    #region Animation
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    private bool isMoving;

    private void AnimatePlayer(Vector2 moveDir)
    {
        Debug.Log("Animation");
        if (isMoving)
        {
            string direction = GetDirection(moveDir);

            animator.SetBool("isMoving", true);

            switch (direction)
            {
                case "right":
                    sprite.flipX = false;
                    animator.SetInteger("state", 1);
                    break;
                case "left":
                    sprite.flipX = true;
                    animator.SetInteger("state", 2);
                    break;
                case "up":
                    animator.SetInteger("state", 3);
                    break;
                case "down":
                    animator.SetInteger("state", 4);
                    break;
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetInteger("state", 0);
        }
    }

    private string GetDirection(Vector2 moveDir)
    {
        moveDir = moveDir.normalized;
        if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
        {
            return moveDir.x > 0 ? "right" : "left";
        }
        else
        {
            return moveDir.y > 0 ? "up" : "down";
        }
    }

    #endregion


    #region Movement

    /// <summary>
    /// Attempt to move the player
    /// </summary>
    public void TryMove()
    {
        // Don't try to move if the CharacterController is disabled
        if (!cc.enabled) return;

        if (moveHeld)
        {
            speedUp();
        }
        else
        {
            speedDown();
        }
        speed *= sprintHeld ? sprintMultipler : 1;
        var dir = transform.forward * speed * moveDir.y + transform.right * speed * moveDir.x;
        cc.Move(dir * Time.deltaTime);
    }

    /// <summary>
    /// Called when a movement action updated
    /// </summary>
    /// <param name="ctx">The context of the input action</param>
    public void OnMove(InputAction.CallbackContext ctx)
    {
        // Don't process movement when shop or inventory is open
        if (ShopManager.GetInstance().IsOpen() || InventoryManager.GetInstance().IsInventoryOpen())
        {
            moveDir = Vector2.zero;
            moveHeld = false;
            return;
        }

        if (ctx.canceled)
        {
            moveHeld = false;
            moveDir = Vector2.zero;
        }
        else
        {
            moveHeld = true;
            moveDir = ctx.ReadValue<Vector2>();
        }
        AnimatePlayer(moveDir);
    }

    /// <summary>
    /// Called when sprint action updated
    /// </summary>
    /// <param name="ctx">The context of the input action</param>
    public void OnSprint(InputAction.CallbackContext ctx)
    {
        sprintHeld = !ctx.canceled;
    }

    /// <summary>
    /// Increase the player's speed until speed reaches maxSpeed
    /// </summary>
    void speedUp()
    {
        if (speed == 0)
        {
            speed = 0.1f;
        }
        speed *= acceleration;
        if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }
    }

    /// <summary>
    /// Slow down the player until the speed is 0
    /// </summary>
    void speedDown()
    {
        speed *= deceleration;
        if (speed <= 0.01)
        {
            speed = 0;
        }
    }

    /// <summary>
    /// Called when a look action updated
    /// </summary>
    /// <param name="ctx">The context of the input action</param>
    public void OnLook(InputAction.CallbackContext ctx)
    {
        // Don't process look input when shop or inventory is open
        if (ShopManager.GetInstance().IsOpen() || InventoryManager.GetInstance().IsInventoryOpen())
            return;

        var rot = ctx.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, rot.x * Time.deltaTime);
    }
    #endregion

    #region Interaction
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private int _numFound;
    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private GameObject interactableCanvas;

    /// <summary>
    /// Called when Interact action is performed.
    /// </summary>
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (ShopManager.GetInstance().IsOpen() || InventoryManager.GetInstance().IsInventoryOpen())
                return;

            if (_numFound > 0)
            {
                var interactable = _colliders[0].GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(this);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC" || other.tag == "Plant")
        {
            if (ShopManager.GetInstance().ShopPanelRect.gameObject.activeSelf)
            {
                Debug.Log("Exit Trigger");
                StartCoroutine(HandleShopCloseOnExit());
                interactableCanvas.SetActive(false);
            }
        }
    }

    private IEnumerator HandleShopCloseOnExit()
    {
        var operation = ShopManager.GetInstance().ShopOutro();
        while (!operation.IsCompleted)
        {
            yield return null;
        }
        EnablePlayerMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC" || other.tag == "Plant")
        {
            Debug.Log("Enter Trigger");
            interactableCanvas.SetActive(true);
        }
    }

    /// <summary>
    /// Draws WireSpheres of the Interaction radius
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
    #endregion

    #region Planting
    public void OnPlantSeed(InputAction.CallbackContext ctx)
    {
        if (ShopManager.GetInstance().IsOpen() || InventoryManager.GetInstance().IsInventoryOpen())
            return;

        if (ctx.started)
        {
            UseSelectedItem();
        }
    }

    public void UseSelectedItem()
    {
        Item receivedItem = null;
        try
        {
            receivedItem = InventoryManager.GetInstance().GetSelectedItem(true);
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("Invalid/no item retrieved.");
        }

        if (receivedItem.type == ItemType.Bug) return;

        if (receivedItem != null && receivedItem.type == ItemType.Seed)
        {
            AudioManager.GetInstance().PlayPlantSound();
            SeedPlanter planter = GetComponent<SeedPlanter>();
            planter.PlantSeed(receivedItem.seedData, transform.position);
        }
        else
        {
            Debug.Log("No item in hand.");
            return;
        }
    }
    #endregion

    #region Inventory
    public async void OnInventory(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;

        // Don't allow opening inventory when shop is open
        if (ShopManager.GetInstance().IsOpen()) return;

        // Handle both UI elements as awaitable tasks
        await ShopManager.GetInstance().ShopOutro();

        Debug.Log("InventoryPressed");

        if (inventoryUI.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Both methods now return proper awaitable Tasks
            await InventoryManager.GetInstance().InventoryOutro();
            await Task.Delay(50);
            inventoryUI.SetActive(false);
            EnablePlayerMovement();
        }
        else
        {
            DisablePlayerMovement();
            Cursor.lockState = CursorLockMode.None;
            inventoryUI.SetActive(true);
            await Task.Delay(50);
            await InventoryManager.GetInstance().InventoryIntro();
            Cursor.visible = true;
        }
    }

    public void EnablePlayerMovement()
    {
        // Enable character controller and reset movement state
        if (cc != null) cc.enabled = true;
        moveDir = Vector2.zero;
        moveHeld = false;
    }

    public void DisablePlayerMovement()
    {
        // Disable character controller to prevent movement
        if (cc != null) cc.enabled = false;
    }
    #endregion
}

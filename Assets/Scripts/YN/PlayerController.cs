using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

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
    Vector2 moveDir = Vector2.zero;
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

    [SerializeField] GameObject inventoryUI;
    void Start()
    {
        if (cc == null) cc = GetComponent<CharacterController>();
        GetComponent<CameraController>().target = gameObject;
        Cursor.lockState = CursorLockMode.Locked; //lock cursor to play window
        Cursor.visible = false; //make cursor invisible

    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
    }


    #region Movement

    /// <summary>
    /// Attempt to move the player
    /// </summary>
    public void TryMove()
    {
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
        if (ctx.canceled)
        {
            moveHeld = false;
        }
        else
        {
            var newDir = ctx.ReadValue<Vector2>();
            if (newDir.normalized != moveDir.normalized)
            { //if the new direction is different from the old direction
                //speed = 0;
            }
            moveHeld = true;
            moveDir = newDir;
        }
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

    /// <summary>
    /// Called when Interact action is performed.
    /// </summary>
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
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
        if (other.tag == "NPC")
        {
            ShopManager.GetInstance().ShopOutro();
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
    [SerializeField] private GameObject plantPrefab;
    public void OnPlantSeed(InputAction.CallbackContext ctx)
    {
        // possible edge case: what happens when slot is empty or not a bug?
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

        ShopManager.GetInstance().ShopOutro();

        Debug.Log("InventoryPressed");
        if (inventoryUI.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            await InventoryManager.GetInstance().InventoryOutro();
            await Task.Delay(50);
            inventoryUI.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            inventoryUI.SetActive(true);
            await Task.Delay(50);
            InventoryManager.GetInstance().InventoryIntro();
            Cursor.visible = true;
        }
    }
    #endregion
}

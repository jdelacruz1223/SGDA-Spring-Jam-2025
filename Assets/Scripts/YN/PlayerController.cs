using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{  
    /// <summary>
    /// Maximum speed player can reach
    /// </summary>
    [SerializeField] float maxSpeed;
    /// <summary>
    /// Current speed of player
    /// </summary>
    float speed;
    /// <summary>
    /// Are the movement buttons held?
    /// </summary>
    bool moveHeld = false;
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
    void Start()
    {
        if(cc == null) cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
    }

#region Movemnent
    public void TryMove(){
        if(moveHeld){
            speedUp();
        }
        else{
            speedDown();
        }
        var dir = new Vector3(speed*moveDir.x, 0 , speed*moveDir.y);
        cc.Move(dir*Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx){
        if(ctx.canceled){
            moveHeld = false;
        }
        else{
            var newDir = ctx.ReadValue<Vector2>();
            if (newDir.normalized != moveDir.normalized){ //if the new direction is different from the old direction
                //speed = 0;
            }
            moveHeld = true;
            moveDir = newDir;
        }
    }

    public void speedUp(){
        if(speed >= maxSpeed){
            speed = maxSpeed;
            return;
        }
        speed += acceleration; //TODO this is linear only because of the edge case that speed can be 0 prevents 'acceleration' from being exponential like deceleartion
    }

    public void speedDown(){
        if(speed <= 0.01){
            speed = 0;
            return;
        }
        speed *= deceleration;
    }
    #endregion

    #region Inventory
    public void OnInventory(InputAction.CallbackContext ctx) {
        //control which seed is chosen
    }
    #endregion

}

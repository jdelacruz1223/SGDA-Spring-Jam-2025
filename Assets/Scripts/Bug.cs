using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bug : MonoBehaviour
{
    public enum BugType { FernBug }
    public BugType currentBugType; 
    public int value;

    public void Initialize(Plant.PlantType plantType, int bugValue) {
        switch(plantType) {
            case Plant.PlantType.Fern:
                currentBugType = BugType.FernBug;
                break;
            default: 
                currentBugType = BugType.FernBug;
                break;  
        }
        value = bugValue;
    }

    void Start()
    {
        if(rb == null) rb = GetComponent<Rigidbody>();
        StartCoroutine(MoveCycle());
    }


    private void StartMinigame() {
        Debug.Log("Minigame hajimeru");
    }

    #region Movement
        Rigidbody rb;
        float speed;
        [SerializeField] float maxSpeed;
        [SerializeField] float acceleration;
        [SerializeField] float deceleration;

        bool isMoving;
        bool isDragging;
        Vector3 moveDir;
        [SerializeField] float directionChangeInterval;
        [SerializeField] float pauseInterval;

        void FixedUpdate()
        {
            if(isDragging){
                DragBug();
            }
            if(isMoving){
                SpeedUp();
            }
            else{
                SpeedDown();
            }
            rb.MovePosition(transform.position+moveDir*speed*Time.deltaTime);
        }

        void ChangeDir(){
            float randX = Random.Range(-1f, 1f);
            float randY = Random.Range(-1f, 1f);
            moveDir = new Vector3(randX, randY, 0).normalized;
        }

        IEnumerator MoveCycle(){
            while(true){
                if(isDragging){
                    yield return new WaitForEndOfFrame();
                }
                isMoving = true;
                ChangeDir();
                yield return new WaitForSeconds(directionChangeInterval);
                isMoving = false;
                yield return new WaitForSeconds(pauseInterval);
            }
        }

        void SpeedUp(){
            if(speed == 0){
                speed = 0.1f;
            }
            speed *= acceleration;
            if(speed >= maxSpeed){
                speed = maxSpeed;
            }
        }

        void SpeedDown(){
            speed *= deceleration;
            if(speed <= 0.01){
                speed = 0;
            }
        }

        public void OnClick(InputAction.CallbackContext ctx){
            isDragging = !ctx.canceled;
        }

        void DragBug(){
            //set position of bug to follow mouse pointer? 
        }
    #endregion
}

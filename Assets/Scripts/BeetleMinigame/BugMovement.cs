using UnityEngine;

public class BugMovement : MonoBehaviour
{
    [SerializeField] float moveForce = 500;
    [SerializeField] Rigidbody rb;

    Vector3 moveInput = Vector3.zero;

    public void UpdateMoveVector(Vector3 mv)
    {
        moveInput = mv;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tree Goal") {
            // PUT WIN STATE HERE
            Debug.Log("I win");
        }
        else {
            // PUT LOSE STATE HERE
            Debug.Log("I lose");
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(moveInput * moveForce);
    }
}

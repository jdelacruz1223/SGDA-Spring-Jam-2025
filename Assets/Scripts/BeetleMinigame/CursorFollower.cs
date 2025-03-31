using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] float dragFollowSpeed = 0.5f;

    float followSpeed;

    public Vector3 GetBeetleMoveVector()
    {
        Vector3 moveVector = Input.mousePosition - transform.position;
        return moveVector;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) { // if player is holding LMB
            followSpeed = dragFollowSpeed;
        }
        else { // otherwise
            followSpeed = 1;
        }

        transform.position = Vector3.Lerp(transform.position, Input.mousePosition, followSpeed);
    }
}

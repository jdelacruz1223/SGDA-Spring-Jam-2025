using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    [Range(0, 1)][SerializeField] float dragFollowSpeed = 0.5f;
    public bool isActive = false;

    float followSpeed;

    public Vector3 GetBeetleMoveVector()
    {
        if (!isActive) return Vector3.zero;
        Vector3 moveVector = Input.mousePosition - transform.position;
        return moveVector;
    }

    void Update()
    {
        if (!isActive) return;

        if (Input.GetMouseButton(0))
        {
            followSpeed = dragFollowSpeed;
        }
        else
        {
            followSpeed = 1;
        }

        transform.position = Vector3.Lerp(transform.position, Input.mousePosition, followSpeed);
    }
}

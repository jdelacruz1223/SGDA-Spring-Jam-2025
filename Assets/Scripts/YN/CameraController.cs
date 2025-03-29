using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Target that camera should move towards
    /// </summary>
    public GameObject target;
    /// <summary>
    /// How far from the target should the camera be?
    /// </summary>
    [SerializeField] float distance;
    /// <summary>
    /// How far above the target should the camera be?
    /// </summary>
    [SerializeField] float height;
    /// <summary>
    /// How smooth should the movement be? Higher values result in slower movement, and lower values in faster movement.
    /// </summary>
    [SerializeField] float smoothing;
    /// <summary>
    /// For smoothening, keeps track of velocity
    /// </summary>
    Vector3 vel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var target_pos = target.transform.position - target.transform.forward * distance + Vector3.up * height;
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target_pos, ref vel, smoothing);
        Camera.main.transform.LookAt(target.transform);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BugMovement : MonoBehaviour
{
    [Header("For Movement")]
    [SerializeField] float moveForce = 500;
    [SerializeField] Rigidbody rb;

    [Header("For Scene Change")]
    [SerializeField] float sceneChangeDelay = 2f;
    [SerializeField] string winText = "YOU CAUGHT THE BUG!";
    [SerializeField] string loseText = "YOU FAILED TO CATCH IT!";
    [SerializeField] TextMeshProUGUI endText;
    [SerializeField] string readyText = "GET READY AND PLACE CURSOR ON BUG!";

    Vector3 moveInput = Vector3.zero;
    public bool isDone = false;
    public bool isReady = false;

    void Start()
    {
        if (endText != null)
        {
            endText.text = readyText;
        }
    }

    public void UpdateMoveVector(Vector3 mv)
    {
        moveInput = mv;
    }

    public void TimeOut()
    {
        if (isDone) return;

        isDone = true;
        endText.text = loseText;
    }

    public void SetReady()
    {
        isReady = true;
        if (endText != null)
        {
            endText.text = "";
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDone) return;

        isDone = true;

        if (collision.gameObject.tag == "Tree Goal")
        { // player wins
            endText.text = winText;
            GameDataManager.GetInstance().AddBug(GameDataManager.GetInstance().currentBug.id);
        }
        else
        { // player loses
            endText.text = loseText;
        }

        GameDataManager.GetInstance().currentBug = null;
    }

    void FixedUpdate()
    {
        if (isDone || !isReady) return;
        rb.AddForce(moveInput * moveForce);
    }
}

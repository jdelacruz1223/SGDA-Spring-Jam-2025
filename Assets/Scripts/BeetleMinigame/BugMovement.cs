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

    Vector3 moveInput = Vector3.zero;
    bool isDone = false;

    public void UpdateMoveVector(Vector3 mv)
    {
        moveInput = mv;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDone) return;

        isDone = true;

        if (collision.gameObject.tag == "Tree Goal") { // player wins
            endText.text = winText;
            //GameDataManager.GetInstance().AddBug(GameDataManager.GetInstance().currentBug.id);
        }
        else { // player loses
            endText.text = loseText;
        }

        Invoke("ReturnToMainGame", sceneChangeDelay);
    }

    void FixedUpdate()
    {
        if (isDone) return;
        rb.AddForce(moveInput * moveForce);
    }

    void ReturnToMainGame()
    {
        SceneManager.LoadScene("Garden");
    }
}

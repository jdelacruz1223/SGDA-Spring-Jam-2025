using UnityEngine;
using System.Collections;

public class BeetleMinigameManager : MonoBehaviour
{
    public BugMovement beetleObject;
    public RectTransform beetleSprite;
    public CursorFollower cursorFollower;

    [SerializeField] private float startDelay = 1.5f;
    private bool isReady = false;

    void Start()
    {
        if (beetleObject == null)
            beetleObject = GetComponentInChildren<BugMovement>();

        if (beetleSprite == null)
            beetleSprite = GetComponentInChildren<RectTransform>();

        if (cursorFollower == null)
            cursorFollower = GetComponentInChildren<CursorFollower>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (cursorFollower != null)
        {
            cursorFollower.isActive = false;

            if (beetleSprite != null)
            {
                Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
                cursorFollower.transform.position = screenCenter;
                beetleSprite.position = new Vector3(screenCenter.x, screenCenter.y, beetleSprite.position.z);
                StartCoroutine(ActivateAfterDelay());
            }
        }
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        if (cursorFollower != null)
        {
            cursorFollower.isActive = true;
            isReady = true;

            if (beetleObject != null)
            {
                beetleObject.SetReady();
            }
        }
    }

    void Update()
    {
        if (beetleObject == null || beetleSprite == null || cursorFollower == null)
            return;

        Vector3 newBeetleSpritePos = new Vector3(beetleObject.transform.position.x,
                                                beetleObject.transform.position.y,
                                                beetleSprite.position.z);
        beetleSprite.position = newBeetleSpritePos;

        beetleObject.UpdateMoveVector(cursorFollower.GetBeetleMoveVector());
    }
}

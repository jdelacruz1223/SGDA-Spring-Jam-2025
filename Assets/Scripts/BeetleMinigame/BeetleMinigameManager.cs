using UnityEngine;

public class BeetleMinigameManager : MonoBehaviour
{
    [SerializeField] BugMovement beetleObject;
    [SerializeField] RectTransform beetleSprite;
    [SerializeField] CursorFollower cursorFollower;

    void Update()
    {
        Vector3 newBeetleSpritePos = new Vector3(beetleObject.transform.position.x,
                                                 beetleObject.transform.position.y,
                                                 beetleSprite.position.z);
        beetleSprite.position = newBeetleSpritePos;

        // e

        beetleObject.UpdateMoveVector(cursorFollower.GetBeetleMoveVector());
    }
}

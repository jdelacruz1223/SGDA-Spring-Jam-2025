using DG.Tweening;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager GetInstance() { return me; }
    public static AnimationManager me;

    [Header("Miscellaneous")]
    [SerializeField] public RectTransform Toolbar;

    [Header("Settings")]
    public GameObject SettingsUI;

    [Header("Shop Settings")]
    [SerializeField] public RectTransform ShopPanelRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;

    void Awake()
    {
        if (me != null)
        {
            Destroy(gameObject);
            return;
        }

        me = this;
    }

    #region SHOP UI ANIMATION
    public void ShopIntro()
    {
        ShopPanelRect.DOAnchorPosY(middlePosY, tweenDuration);
    }

    public void ShopOutro()
    {
        ShopPanelRect.DOAnchorPosY(topPosY, tweenDuration);
    }
    #endregion
}
using TMPro;
using UnityEngine;

public class ToggleInstructions : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI howToShowText;
    [SerializeField] string pressToShow = "Press Tab to show instructions";
    [SerializeField] string pressToHide = "Press Tab to hide instructions";
    bool isPressToShow;

    void Start()
    {
        ChangeToPressToShow();
        isPressToShow = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (isPressToShow) {
                ChangeToPressToHide();
                isPressToShow = false;
            }
            else {
                ChangeToPressToShow();
                isPressToShow = true;
            }
        }
    }

    void ChangeToPressToShow()
    {
        howToShowText.text = pressToShow;
    }

    void ChangeToPressToHide()
    {
        howToShowText.text = pressToHide;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInstructions : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI instructions;
    [SerializeField] TextMeshProUGUI howToShowText;
    [SerializeField] Image instructionBackground;

    [Header("Text to Show")]
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

        instructions.enabled = false;
        instructionBackground.enabled = false;
    }

    void ChangeToPressToHide()
    {
        howToShowText.text = pressToHide;

        instructions.enabled = true;
        instructionBackground.enabled = true;
    }
}

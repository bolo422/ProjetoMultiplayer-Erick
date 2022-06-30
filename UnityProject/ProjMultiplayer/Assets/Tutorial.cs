using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language { portuguese, english }
public class Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject menuBackgroundImage, firstMenu, serverConfig, panelWarningMenu, tutorialPanelObject;

    [SerializeField]
    GameObject nextButton, backButton, finishTutorialButton, backToMenuButton;

    [SerializeField]
    GameObject tutorial2, tutorial3;

    [SerializeField]
    private UnityEngine.UI.Text instructionsText;

    private int currentTutorialIndex = 0;

    private void OnEnable()
    {
        menuBackgroundImage.SetActive(false);
        UpdateText();
        UpdateButtons();
    }

    private void OnDisable()
    {
        menuBackgroundImage.SetActive(true);
    }

    public void SwitchLanguage()
    {
        TutorialTexts.SwitchLanguage();
        UpdateText();
    }

    public void Next()
    {
        if(currentTutorialIndex < TutorialTexts.LastIndex())
        {
            currentTutorialIndex++;
            UpdateText();
        }
        
        UpdateButtons();
    }

    public void Back()
    {
        if (currentTutorialIndex > 0)
        {
            currentTutorialIndex--;
            UpdateText();
        }

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (currentTutorialIndex == 0)
        {
            backToMenuButton.SetActive(true);
            backButton.SetActive(false);
        }
        else
        {
            backToMenuButton.SetActive(false);
            backButton.SetActive(true);
        }

        if (currentTutorialIndex == TutorialTexts.LastIndex())
        {
            finishTutorialButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else
        {
            finishTutorialButton.SetActive(false);
            nextButton.SetActive(true);
        }
    }

    private void UpdateText()
    {
        instructionsText.text = TutorialTexts.GetTutorial(currentTutorialIndex);

        if (currentTutorialIndex == 1)
            tutorial2.SetActive(true);
        else
            tutorial2.SetActive(false);

        if (currentTutorialIndex == 2)
            tutorial3.SetActive(true);
        else
            tutorial3.SetActive(false);
    }

    public void FinishTutorial()
    {
        StartingArguments.Instance.StartClient();
    }

    public void BackToMenu()
    {
        menuBackgroundImage.SetActive(true); firstMenu.SetActive(true); serverConfig.SetActive(false); panelWarningMenu.SetActive(false); tutorialPanelObject.SetActive(false);
    }
}

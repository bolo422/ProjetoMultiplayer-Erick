using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language { portuguese, english }
public class Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject menuBackgroundImage, firstMenu, serverConfig, panelWarningMenu, tutorialPanelObject, firstTutorial, secondTutorial;

    [SerializeField]
    private UnityEngine.UI.Text instructionsText;

    private void SwitchMenu(GameObject activeObject)
    {
        GameObject[] go = new[] { menuBackgroundImage, firstMenu, serverConfig, panelWarningMenu, tutorialPanelObject };

        foreach(GameObject g in go)
        {
            if (g == activeObject)
                g.SetActive(true);
            else
                g.SetActive(false);
        }
    }

    private Language language = Language.portuguese;

    private string pt, en;

    private void OnEnable()
    {
        pt = " ";
        en = " ";
        menuBackgroundImage.SetActive(false); firstMenu.SetActive(false); serverConfig.SetActive(false); panelWarningMenu.SetActive(false);

    }

    public void SwitchLanguage()
    {
        language = language == Language.portuguese ? Language.english : Language.portuguese;
        instructionsText.text = language == Language.portuguese ? pt : en;
    }

    public void BackTo_Menu()
    {
        SwitchMenu(firstMenu);
    }

    public void NextTo_SecondTutorial()
    {
        pt = " ";
        en = " ";
    }

    public void BackTo_FirstTutorial()
    {
        pt = " ";
        en = " ";
    }

    public void NextTo_ThirdTutorial()
    {

    }


}

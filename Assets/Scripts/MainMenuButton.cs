using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] MainMenuBtnType type;
    public void OnClick()
    {
        switch(type)
        {
            case MainMenuBtnType.Start:
            FadeInOutManager.Instance.FadeIn();
            SceneManager.LoadSceneAsync("Sandbox");
            break;
            case MainMenuBtnType.Exit:
            Application.Quit();
            break;
        }
    }

    public void test()
    {

    }
}

public enum MainMenuBtnType
{
    Start,
    Exit
}

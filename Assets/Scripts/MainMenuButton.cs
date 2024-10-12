using System.Collections;
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
            FadeInOutManager.Instance.SceneLoadWithFade("Sandbox");
            break;
            case MainMenuBtnType.Continue:
            FadeInOutManager.Instance.SceneLoadWithFade("Sandbox", true);
            break;
            case MainMenuBtnType.Exit:
            Application.Quit();
            break;
        }
    }
}

public enum MainMenuBtnType
{
    Start,
    Continue,
    Exit
}

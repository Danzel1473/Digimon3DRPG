using UnityEngine;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject rootMenu;
    [SerializeField] private UnityEngine.GameObject moveMenu;
    private UnityEngine.GameObject currentMenu;

    private void Start()
    {
        currentMenu = rootMenu;
    }

    public void SwitchToMoveMenu()
    {
        if (currentMenu != null) currentMenu.SetActive(false);
        moveMenu.SetActive(true);
        currentMenu = moveMenu;
    }

    public void SwitchToRootMenu()
    {
        if (currentMenu != null) currentMenu.SetActive(false);
        rootMenu.SetActive(true);
        currentMenu = rootMenu;
    }

    public void HideCurrentMenu()
    {
        if (currentMenu != null) currentMenu.SetActive(false);
    }

    public void ShowCurrentMenu()
    {
        if (currentMenu != null) currentMenu.SetActive(true);
    }
}
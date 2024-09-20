using UnityEngine;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject rootMenu;
    [SerializeField] private GameObject moveMenu;
    private GameObject currentMenu;

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
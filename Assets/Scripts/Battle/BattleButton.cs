using UnityEngine;

public class BattleButton : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Move move;
    [SerializeField] GameObject activeMenu;

    public void ActiveMenu()
    {
        battleSystem.currentMenu.SetActive(false);
        activeMenu.SetActive(true);
        battleSystem.currentMenu = activeMenu;
    }
}
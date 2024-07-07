using UnityEngine;

public class BattleButton : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private Move move;
    [SerializeField] private GameObject activeMenu;

    public void ActiveMenu()
    {
        battleSystem.currentMenu.SetActive(false);
        activeMenu.SetActive(true);
        battleSystem.currentMenu = activeMenu;
    }

    public void OnMoveSelected()
    {
        battleSystem.PlayerMove(move);
    }
}
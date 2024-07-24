using UnityEngine;

public class BattleButton : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private Move move;
    [SerializeField] private GameObject activeMenu;

    public void ActivateMenu()
    {
        battleSystem.SwitchMenu(activeMenu);
    }

    public void OnMoveSelected()
    {
        battleSystem.PlayerMove(move);
    }
}
using UnityEngine;

public class BattleButton : MonoBehaviour
{
    [SerializeField] private GameObject activeMenu;

    public void ActivateMenu()
    {
        BattleSystem.Instance.SwitchMenu(activeMenu);
    }

    public void RunBattle()
    {
        BattleSystem.Instance.BattleRun();
    }
}
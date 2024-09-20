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
        //임시 코드
        StartCoroutine(BattleSystem.Instance.BattleText("도망쳤다.", 2f));
        
    }
}
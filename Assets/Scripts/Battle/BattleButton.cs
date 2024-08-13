using UnityEngine;

public class BattleButton : MonoBehaviour
{
    [SerializeField] private GameObject activeMenu;
    private BattleSystem battleSystem;

    public void Awake()
    {
        battleSystem = GameObject.FindWithTag("BattleManager").GetComponent<BattleSystem>();
    }

    public void ActivateMenu()
    {
        battleSystem.SwitchMenu(activeMenu);
    }

    public void RunBattle()
    {
        //임시 코드
        StartCoroutine(battleSystem.BattleText("도망쳤다.", 2f));
    }
}
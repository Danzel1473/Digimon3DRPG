using UnityEngine;

public class BattleAnimation : MonoBehaviour
{
    private BattleSystem battleSystem;

    private void Start()
    {
        battleSystem = GameObject.FindWithTag("BattleManager").GetComponent<BattleSystem>();
    }

    public void AttackTiming()
    {
    }

    public void DefenseTiming()
    {
    }
}
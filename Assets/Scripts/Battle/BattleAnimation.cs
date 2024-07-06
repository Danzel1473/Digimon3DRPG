using Unity.VisualScripting;
using UnityEngine;

public class BattleAnimation : MonoBehaviour
{
    BattleSystem battleSystem;

    public void Start()
    {
        battleSystem = GameObject.FindWithTag("BattleManager").GetComponent<BattleSystem>();
    }
    
    public void AttackTiming()
    {
        battleSystem.AfterAttackTiming();
    }
}
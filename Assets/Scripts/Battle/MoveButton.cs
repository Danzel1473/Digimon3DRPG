using UnityEngine;

public class MoveButton : MonoBehaviour
{
    private BattleSystem battleSystem;
    private Move move;

    public void Start()
    {
        battleSystem = GameObject.FindWithTag("BattleManager").GetComponent<BattleSystem>();
    }

    public void SetUp(MoveBase moveBase)
    {
        if (moveBase == null) return;
        move = new Move(moveBase);
    }

    public void OnClick()
    {
        if (move == null) return;
        battleSystem.PlayerMove(move);
    }
}
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    private Move move;

    public void SetUp(MoveBase moveBase)
    {
        if (moveBase == null) return;
        move = new Move(moveBase);
    }

    public void OnClick()
    {
        if (move == null) return;
        BattleSystem.Instance.PlayerMove(move);
    }
}
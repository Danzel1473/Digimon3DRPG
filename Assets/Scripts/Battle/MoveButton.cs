using UnityEngine;

public class MoveButton : MonoBehaviour
{
    private Move move;

    public void SetUp(int moveNum)
    {
        move = new Move(moveNum);
    }

    public void OnClick()
    {
        if (move == null) return;
        BattleSystem.Instance.PlayerMove(move);
    }
}
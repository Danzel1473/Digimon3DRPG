using UnityEngine;

public class MoveButton : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    Move move;

    
    public void SetUp(MoveBase moveb)
    {
        if(moveb == null) return;
        move = new Move(moveb);
    }

    public void onClicked()
    {
        if(move == null) return;
        battleSystem.PlayerMove(move);
    }

}
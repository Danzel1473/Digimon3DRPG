using System.Collections.Generic;
using UnityEngine;

public class Digimon : MonoBehaviour
{
    public DigimonData digimonData;
    public int level;
    public int currentHP;
    public List<Move> moves;

    private void Start()
    {
        if (moves == null)
        {
            moves = new List<Move>();
        }
    }

    public void LearnMove(Move newMove)
    {
        if (moves.Count < 4)
        {
            moves.Add(newMove);
        }
        else
        {
        //DO TO : 기술이 4개 있다면 기술을 1개 지우고 기술을 배운다
            ReplaceMove(newMove); 
        }
    }

    private void ReplaceMove(Move newMove)
    {
    }

    public void LevelUp()
    {
        level++;

        Move newMove = CheckForNewMove();
        if (newMove != null)
        {
            LearnMove(newMove);
        }
    }

    private Move CheckForNewMove()
    {
        // DO TO : 레벨에 따라 새로운 기술을 배울지 체크
        return null;
    }
}
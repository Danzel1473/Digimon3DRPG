using System.Collections.Generic;
using UnityEngine;

public class Digimon
{
    public DigimonBase digimonBase { get; private set; }
    public int level { get; private set; }
    public int HP { get; set; }
    public List<Move> Moves { get; private set; }

    public Digimon(DigimonBase digimonBase, int level)
    {
        this.digimonBase = digimonBase;
        this.level = level;
        HP = digimonBase.MaxHP;

        Moves = new List<Move>();
        foreach (var move in digimonBase.LearnableMoves)
        {
            if (move.level <= level)
                Moves.Add(new Move(move.moveBase));
        }
    }

    public int Attack => Mathf.FloorToInt((digimonBase.Attack * level) / 100f) + 5;
    public int Defense => Mathf.FloorToInt((digimonBase.Defense * level) / 100f) + 5;
    public int Speed => Mathf.FloorToInt((digimonBase.Speed * level) / 100f) + 5;
}
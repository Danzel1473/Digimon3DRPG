using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Digimon
{
    public DigimonBase digimonBase { get; private set; }
    public int Level { get; private set; }
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public List<Move> Moves { get; private set; }
    public int VirusValue, DataValue, VaccineValue;

    public Digimon(DigimonBase digimonBase, int level)
    {
        this.digimonBase = digimonBase;
        Level = level;
        MaxHP = HP;
        CurrentHP = MaxHP;

        Moves = new List<Move>();
        foreach (var move in digimonBase.LearnableMoves)
        {
            if (move.level <= level)
                Moves.Add(new Move(move.moveBase));
        }
    }

    public Digimon(DigimonBase digimonBase, int level, int currentHP, List<Move> moves)
    {
        this.digimonBase = digimonBase;
        Level = level;
        CurrentHP = currentHP;
        Moves = moves;

        Moves = new List<Move>();
        foreach (var move in digimonBase.LearnableMoves)
        {
            if (move.level <= level)
                Moves.Add(new Move(move.moveBase));
        }
    }
    public int HP => Mathf.FloorToInt(((digimonBase.HP * 2) + 100 ) * Level / 100f) + 10;
    public int Attack => Mathf.FloorToInt((digimonBase.Attack * 2) * Level / 100f) + 5;
    public int Defense => Mathf.FloorToInt((digimonBase.Defense * 2) * Level / 100f) + 5;
    public int Speed => Mathf.FloorToInt((digimonBase.Speed * 2) * Level / 100f) + 5;
}
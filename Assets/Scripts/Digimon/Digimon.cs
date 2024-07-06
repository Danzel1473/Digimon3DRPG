using System.Collections.Generic;
using UnityEngine;

public class Digimon
{
    public DigimonBase digimonBase { get; set; } // 기본 데이터 참조
    public int level { get; set; }
    public int HP { get; set; }
    public List<Move> Moves { get; set; }

    public Digimon(DigimonBase digimonBase, int digimonLevel)
    {
        this.digimonBase = digimonBase;
        level = digimonLevel;
        HP = digimonBase.MaxHP;

        Moves = new List<Move>();
        foreach (var move in digimonBase.LearnableMoves)
        {
            if(Moves.Count >= 4) break;
            if(move.level <= level)
            {
                Moves.Add(new Move(move.moveBase));
            }

        }
    }


    public int MaxHP
    {
        get { return Mathf.FloorToInt((digimonBase.Attack * MaxHP) / 100f) + 10; }
    }
    public int Attack
    {
        get { return Mathf.FloorToInt((digimonBase.Attack * level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((digimonBase.Defense * level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((digimonBase.SpAttack * level) / 100f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((digimonBase.SpDefense * level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((digimonBase.Speed * level) / 100f) + 5; }
    }

    // public bool TakeDamage(Move move, Digimon attacker)
    // {
    //     float modifiers = Random.Range(0.85f, 1f);
    //     float a = (2 * attacker.level + 10) / 250f;
    //     float d = a * move.moveBase.Power * ((float)attacker.Attack / Defense) + 2;
    //     int damage = Mathf.FloorToInt(d * modifiers);

    //     int randomValue = Random.Range(0, 100);
    //     if(move.moveBase.Accuracy > randomValue)
    //     {
    //         return false;
    //     }

    //     HP -= Mathf.Min(HP, damage);
        
    //     if (HP <= 0)
    //     {
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }
}
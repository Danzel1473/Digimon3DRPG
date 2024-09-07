using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Digimon
{
    [SerializeField] public int digimonID;
    public DigimonBase DigimonBase => DigimonTable.Instance[digimonID];

    [SerializeField] public string digimonName;
    public int Level;
    public int CurrentHP;
    public int MaxHP;
    public bool XAnityBody;

    public List<Move> Moves;
    public int VirusValue, DataValue, VaccineValue;

    // public void Initialize(int id)
    // {
    //     SetDigimonByID(id);
    // }
    
    // public void SetDigimonByID(int id)
    // {
    //     digimonID = id;
    //     digimonName = DigimonTable.Instance[id].DigimonName;
    // }

    public Digimon(int id, int level)
    {
        digimonID = id;
        digimonName = DigimonBase.DigimonName;
        Level = level;
        MaxHP = HP;
        CurrentHP = MaxHP;
        XAnityBody = DigimonBase.XAnityBody;
        //디지몬마다 고유번호 가지도록 추가 예정

        Moves = new List<Move>();
        foreach (var move in DigimonBase.LearnableMoves)
        {
            if (move.level <= level) Moves.Add(new Move(move.moveBase));
        }
    }

    public Digimon(int id, int level, int currentHP, List<Move> moves)
    {
        digimonID = id;

        Level = level;
        CurrentHP = currentHP;
        Moves = moves;

        Moves = new List<Move>();
        foreach (var move in DigimonBase.LearnableMoves)
        {
            if (move.level <= level)
                Moves.Add(new Move(move.moveBase));
        }
    }

    public void HealDigimon(int amount)
    {
        CurrentHP += Mathf.Min(MaxHP-CurrentHP, amount);
    }
    
    public List<DigimonBase> GetEvolves()
    {
        List<DigimonBase> canEvolves = new List<DigimonBase>();
        foreach(EvolveData evolveData in DigimonBase.EvoleData)
        {
            DigimonBase evolveTo = DigimonTable.Instance[evolveData.evolveDigimonNum];
            if(evolveData.canEvole(this)) canEvolves.Add(evolveTo);
        }
        return canEvolves;
    }

    public void EvolveTo(List<DigimonBase> canEvolves)
    {
        int i = Random.Range(0, canEvolves.Count);
        
        //canEvolves의 i번째로 진화
        //digimonBase = canEvolves[i];

    }

    public int HP => Mathf.FloorToInt(((DigimonBase.HP * 2) + 100 ) * Level / 100f) + 10;
    public int Attack => Mathf.FloorToInt(DigimonBase.Attack * 2 * Level / 100f) + 5;
    public int Defense => Mathf.FloorToInt(DigimonBase.Defense * 2 * Level / 100f) + 5;
    public int SpAttack => Mathf.FloorToInt(DigimonBase.SpAttack * 2 * Level / 100f) + 5;
    public int SpDefense => Mathf.FloorToInt(DigimonBase.SpDefense * 2 * Level / 100f) + 5;
    public int Speed => Mathf.FloorToInt(DigimonBase.Speed * 2 * Level / 100f) + 5;
}
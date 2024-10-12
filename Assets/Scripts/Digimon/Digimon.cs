using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Digimon
{
    [SerializeField] public int digimonID;
    public DigimonBase DigimonBase => DigimonTable.Instance[digimonID];

    [SerializeField] public string digimonName;
    [SerializeField] public int Level;
    private ElementType[] types;
    private int currentHP;
    private int maxHP;
    [SerializeField] private bool xAnityBody;

    [Range(0, 32)][SerializeField] private int[] ivs = new int[6]; //개체치
    [Range(0, 255)] [SerializeField] private int[] effort = new int[6]; //노력치

    public int[] Ivs => ivs;
    public int[] Effort => effort;
    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;
    public bool XAnityBody => xAnityBody;
    public ElementType[] Types => types;
    [SerializeField] public List<Move> Moves;
    
    public void Initialize()
    {
        if (string.IsNullOrEmpty(digimonName)) digimonName = DigimonBase.DigimonName;
        types = DigimonBase.ElementTypes;
        maxHP = HP;
        currentHP = maxHP;
    }

    public Digimon(int id, int level)
    {
        digimonID = id;
        
        digimonName = DigimonBase.DigimonName;
        Level = level;
        maxHP = HP;
        currentHP = maxHP;
        xAnityBody = DigimonBase.XAnityBody;
        //디지몬마다 고유ID 가지도록 추가 예정

        Moves = new List<Move>();
        foreach (var move in DigimonBase.LearnableMoves)
        {
            if (move.level <= level) Moves.Add(new Move(move.moveID));
        }
    }

    public Digimon(int id, int level, int hp, List<Move> moves)
    {
        digimonID = id;

        Level = level;
        currentHP = hp;
        Moves = moves;

        Moves = new List<Move>();
        foreach (var move in DigimonBase.LearnableMoves)
        {
            if (move.level <= level)
                Moves.Add(new Move(move.moveID));
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= Mathf.Min(currentHP, amount);
        Debug.Log($"{currentHP}");

    }

    public void HealDigimon(int amount)
    {
        currentHP += Mathf.Min(maxHP-currentHP, amount);
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
    }

    public int HP => Mathf.FloorToInt(((DigimonBase.HP * 2) + 100 ) * Level / 100f) + 10;
    public int Attack => Mathf.FloorToInt(DigimonBase.Attack * 2 * Level / 100f) + 5;
    public int Defense => Mathf.FloorToInt(DigimonBase.Defense * 2 * Level / 100f) + 5;
    public int SpAttack => Mathf.FloorToInt(DigimonBase.SpAttack * 2 * Level / 100f) + 5;
    public int SpDefense => Mathf.FloorToInt(DigimonBase.SpDefense * 2 * Level / 100f) + 5;
    public int Speed => Mathf.FloorToInt(DigimonBase.Speed * 2 * Level / 100f) + 5;
}
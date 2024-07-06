using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DigimonBase", menuName = "Digimon/Create Digimon Base")]
public class DigimonBase : ScriptableObject
{
    [SerializeField] string digimonName;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] DigimonType digimonType1;
    [SerializeField] ElementType elementType1;
    [SerializeField] ElementType elementType2;
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    [SerializeField] Sprite digimonSprite;
    [SerializeField] GameObject digimonMedel;
    [SerializeField] List<LearnableMove> learnableMoves;


    public string DigimonName
    {
        get{ return digimonName; }
    }

    public string Description
    {
        get{ return description; }
    }

    public DigimonType DigimonType1
    {
        get{ return digimonType1; }
    }

    public ElementType ElementType1
    {
        get { return elementType1; }
    }

        public ElementType ElementType2
    {
        get { return elementType1; }
    }

    public int MaxHP
    {
        get { return maxHP; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }

    public GameObject DigimonModel
    {
        get { return digimonMedel; }
    }
}


public enum DigimonType
{
    Virus,
    Data,
    Vaccine,
    Free,
    None,
    Variable,
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] public MoveBase moveBase;
    [SerializeField] public int level;
}

public enum ElementType
{
    None,
    Normal,
    Fire,
    Water,
    Grass,
    Ice,
    Wind,
    Electric,
    Insect,
    Steel,
    Darkness,
    Light
}
using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public class DigimonBase
{
    [Header ("Digimon Info")]
    [SerializeField] string digimonName;
    [TextArea] [SerializeField] string description;
    [SerializeField] int digimonNum;

    [Header ("Digimon Stats")]
    [SerializeField] int hp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
 
    [Header ("Types")]
    [SerializeField] DigimonType digimonType;
    [SerializeField] ElementType[] elementTypes;
    [SerializeField] bool xAntiBody;

    [Header ("ETC")]
    [SerializeField] EvolveData[] evoleData;
    [SerializeField] List<LearnableMove> learnableMoves;

    [Header ("Visuals")]
    [SerializeField] Sprite digimonSprite;
    [SerializeField] GameObject digimonModel;
    [SerializeField] public AnimatorController digimonSpriteAnimator;


    public string DigimonName => digimonName;
    public string Description => description;
    public int DigimonNum => digimonNum;
    public DigimonType DigimonType => digimonType;
    public ElementType[] ElementTypes => elementTypes;

    public int HP => hp;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;

    public bool XAnityBody => xAntiBody;
    public List<LearnableMove> LearnableMoves => learnableMoves;
    public GameObject DigimonModel => digimonModel;
    public Sprite DigimonSprite => digimonSprite;
    public AnimatorController DigimonSpriteAnimator => digimonSpriteAnimator;
    public EvolveData[] EvoleData => evoleData;
}



[Serializable]
public class LearnableMove
{
    [SerializeField] public int moveID;
    [SerializeField] public int level;
    public MoveBase moveBase => MoveTable.Instance[moveID];
}


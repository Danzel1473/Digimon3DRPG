using System.Collections.Generic;
using UnityEditor.Animations;
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
    [SerializeField] int hp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    [SerializeField] Sprite digimonSprite;
    [SerializeField] GameObject digimonMedel;
    [SerializeField] List<LearnableMove> learnableMoves;
    [SerializeField] public AnimatorController digimonSpriteAnimator;
    [SerializeField] private List<EvolutionData> evolutionData;


    public string DigimonName => digimonName;

    public string Description => description;

    public DigimonType DigimonType1 => digimonType1;

    public ElementType ElementType1 => elementType1;

    public ElementType ElementType2 => elementType2;

    public int HP => hp;

    public int Attack => attack;

    public int Defense => defense;

    public int SpAttack => spAttack;

    public int SpDefense => spDefense;

    public int Speed => speed;

    public List<LearnableMove> LearnableMoves => learnableMoves;

    public GameObject DigimonModel => digimonMedel;

    public Sprite DigimonSprite => digimonSprite;

    public AnimatorController DigimonSpriteAnimator => digimonSpriteAnimator;
}



[System.Serializable]
public class LearnableMove
{
    [SerializeField] public MoveBase moveBase;
    [SerializeField] public int level;
}


using UnityEngine;

[CreateAssetMenu(fileName = "NewDigimonData", menuName = "Digimon/DigimonData")]
public class DigimonData : ScriptableObject
{
    public string digimonName;
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseSpecialAttack;
    public int baseSpecialDefense;
    public int baseSpeed;

    public DigimonType digimonType;
    public Element[] elements;

    public Sprite digimonSprite;
}


//속성 정보1
public enum DigimonType
{
    Virus,
    Vaccine,
    Data,
    Free,
    Variable,
    None
}


//속성 정보2
public enum Element
{
    Fire,
    Water,
    Wind,
    Ice,
    Earth,
    Grass,
    Steel,
    Lightning,
    Light,
    Darkness
}
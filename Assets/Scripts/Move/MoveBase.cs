using UnityEngine;


[CreateAssetMenu(fileName = "New Move", menuName = "Digimon/Create new Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string moveName;

    [SerializeField] string description;

    [SerializeField] ElementType type;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp; //기술의 사용 가능 횟수
    [SerializeField] bool isSpecial;

    public string MoveName
    {
        get { return moveName; }
    }
    
    public string Description
    {
        get { return description; }
    }
    
    public ElementType Type
    {
        get { return type; }
    }
    
    public int Power
    {
        get { return power; }
    }
    
    public int Accuracy
    {
        get { return accuracy; }
    }
    
    public int PP
    {
        get { return pp; }
    }

    public bool IsSpecial
    {
        get { return isSpecial; }
    }
    
}
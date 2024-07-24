using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Digimon/Create New Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] private string moveName;
    [SerializeField] private string description;
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int pp;
    [SerializeField] private ElementType moveType;
    [SerializeField] private MoveCategory moveCategory;
    [SerializeField] private GameObject particlePrefab;

    public string MoveName => moveName;
    public string Description => description;
    public int Power => power;
    public int Accuracy => accuracy;
    public int PP => pp;
    public ElementType MoveType => moveType;
    public MoveCategory MoveCategory => moveCategory;
    public GameObject ParticlePrefab => particlePrefab;
}

public enum MoveCategory
{
    Physical,
    Special,
    Status
}
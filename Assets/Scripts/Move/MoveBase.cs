using System;
using UnityEngine;

[Serializable]
public class MoveBase
{
    [SerializeField] private int moveID;
    [SerializeField] private string moveName;
    [TextArea] [SerializeField] private string description;
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int pp;
    [SerializeField] private ElementType moveType;
    [SerializeField] private MoveTarget moveTarget;
    [SerializeField] private MoveEffect moveEffect;
    [SerializeField] private MoveCategory moveCategory;

    [SerializeField] private GameObject particlePrefab;

    public int MoveID => moveID;
    public string MoveName => moveName;
    public string Description => description;
    public int Power => power;
    public int Accuracy => accuracy;
    public int PP => pp;
    public ElementType MoveType => moveType;
    public MoveTarget MoveTarget => moveTarget;
    public MoveCategory MoveCategory => moveCategory;
    public MoveEffect MoveEffect => moveEffect;

    public GameObject ParticlePrefab => particlePrefab;

    public static explicit operator MoveBase(UnityEngine.Object v)
    {
        throw new NotImplementedException();
    }
}

public enum MoveCategory
{
    Physical,
    Special
}

public enum MoveEffect
{
    Deal,
    Heal,
    Buff,
    Debuff,
    Drain
}

public enum MoveTarget
{
    Enemy,
    User,
    Any
}
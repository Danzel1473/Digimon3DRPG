using System;
using UnityEngine;

public enum ItemAttributeKind
{
    None,
    Heal,
    Evolution,
    Digicatch,
    Buff
}

[Serializable]
public struct ItemAttribute
{
    [SerializeField] private ItemAttributeKind kind;
    [SerializeField] private float value;

    public ItemAttributeKind Kind => kind;
    public float Value => value;
}
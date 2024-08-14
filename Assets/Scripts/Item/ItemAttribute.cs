using System;
using UnityEngine;

public enum ItemAttributeKind
{
    None,
    Heal,
    Evolution,
    Buff
}

[Serializable]
public struct ItemAttribute
{
    [SerializeField] private ItemAttributeKind kind;
    [SerializeField] private int value;

    public ItemAttributeKind Kind => kind;
    public int Value => value;
}
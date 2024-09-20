using System;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField] private int itemId;
    [SerializeField] private string name;
    [SerializeField] private string description;

    [SerializeField] private Sprite icon;
    [SerializeField] private ItemAttribute[] attrs;
    [SerializeField] private ItemKind kind;

    public int ItemId => itemId;
    public string Name => name;
    public string Description => description;


    public Sprite Icon => icon;
    public ItemAttribute[] Attrs => attrs;
    public ItemKind Kind => kind;
}
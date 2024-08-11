using System;
using UnityEngine;

[Serializable]
public class Buff
{
    [SerializeField]
    private int buffId;
    [SerializeField]
    private string name;
    [SerializeField]
    private ItemAttribute[] attrs;
    
    public int BuffId => buffId;
    public string Name => name;
    public ItemAttribute[] Attributes => attrs;
}
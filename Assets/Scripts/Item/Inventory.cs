using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<ItemInstance> items;

    public List<ItemInstance> Items => items;

    public void AddItem(ItemBase item)
    {
    }

    public void RemoveItem(ItemBase item)
    {
    }
}

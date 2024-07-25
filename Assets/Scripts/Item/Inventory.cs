using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<ItemInstance> items;

    public List<ItemInstance> Items => items;

    public Inventory()
    {
        items = new List<ItemInstance>();
    }

    public void AddItem(ItemBase item)
    {
        ItemInstance slot = items.Find(s => s.itemBase == item);
        if (slot != null)
        {
            slot.IncreaseQuantity();
        }
        else
        {
            items.Add(new ItemInstance(item, 1));
        }
    }

    public void RemoveItem(ItemBase item)
    {
        ItemInstance slot = items.Find(s => s.itemBase == item);
        if (slot != null)
        {
            slot.DecreaseQuantity();
            if (slot.quantity <= 0)
            {
                items.Remove(slot);
            }
        }
        else
        {
        }
    }
}

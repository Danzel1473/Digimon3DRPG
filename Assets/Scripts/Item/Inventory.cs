using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<ItemSlot> itemSlots;

    public List<ItemSlot> ItemSlots => itemSlots;

    public Inventory()
    {
        itemSlots = new List<ItemSlot>();
    }

    public void AddItem(ItemBase item)
    {
        ItemSlot slot = itemSlots.Find(s => s.Item == item);
        if (slot != null)
        {
            slot.IncreaseQuantity();
        }
        else
        {
            itemSlots.Add(new ItemSlot(item, 1));
        }
    }

    public void RemoveItem(ItemBase item)
    {
        ItemSlot slot = itemSlots.Find(s => s.Item == item);
        if (slot != null)
        {
            slot.DecreaseQuantity();
            if (slot.Quantity <= 0)
            {
                itemSlots.Remove(slot);
            }
        }
        else
        {
        }
    }
}

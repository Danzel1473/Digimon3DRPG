using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<ItemInstance> items = new List<ItemInstance>();

    public List<ItemInstance> Items => items;
    

    public void AddItem(Item item, int quantity)
    {
        bool find = false;
        if(items.Count > 0)
        {
            foreach(ItemInstance i in items)
            {
                if(i.item == item)
                {
                    find = true;
                    i.quantity += quantity;
                }
            }
        }
        
        if(!find)
        {
            items.Add(new ItemInstance(item.ItemId, quantity));
        }
    }

    public void RemoveItem(Item item, int quantity)
    {
        foreach(ItemInstance i in items)
        {
            if(i.item == item)
            {
                i.quantity -= quantity;
                if(i.quantity <= 0) items.Remove(i);
            }
        }
    }
}

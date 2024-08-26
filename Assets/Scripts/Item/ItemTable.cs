using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Tables/new Item Table")]
public class ItemTable : ScriptableObject, IEnumerable<Item>
{
    private static ItemTable instance;

    public static ItemTable Instance
    {
        get
        {
            if (instance)
                return instance;

            instance = Resources.Load<ItemTable>("Tables/ItemTable");
            instance.Initialize();

            return instance;
        }
    }
    
    [SerializeField] private Item[] items;
    [NonSerialized] private Dictionary<int, Item> itemDict;

    protected void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (itemDict == null)
        {
            itemDict = new Dictionary<int, Item>(items.Select(item => new KeyValuePair<int, Item>(item.ItemId, item)));
        }
    }

    public Item this[int itemId] => itemDict[itemId];
    public int ItemTableLength => items.Length;


    public IEnumerator<Item> GetEnumerator() => items.Cast<Item>().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
}
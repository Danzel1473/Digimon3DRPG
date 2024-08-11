using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffTable", menuName = "Tables/new Buff Table")]
public class BuffTable : ScriptableObject, IEnumerable<Buff>
{
    private static BuffTable instance;

    public static BuffTable Instance
    {
        get
        {
            if (instance)
                return instance;

            instance = Resources.Load<BuffTable>("Tables/BuffTable");
            
            return instance;
        }
    }
    
    [SerializeField]
    private Buff[] buffs;

    [NonSerialized]
    private Dictionary<int, Buff> buffDict;

    protected void Awake()
    {
        buffDict = new Dictionary<int, Buff>(buffs.Select(buff => new KeyValuePair<int, Buff>(buff.BuffId, buff)));
    }

    public Buff this[int buffId] => buffDict[buffId];

    public IEnumerator<Buff> GetEnumerator() => buffs.Cast<Buff>().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => buffs.GetEnumerator();
}
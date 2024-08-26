using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DigimonTable", menuName = "Tables/new Digimon Table")]
public class DigimonTable : ScriptableObject
{
    private static DigimonTable instance;

    public static DigimonTable Instance
    {
        get
        {
            if (instance)
                return instance;

            instance = Resources.Load<DigimonTable>("Tables/DigimonTable");
            instance.Initialize();

            return instance;
        }
    }
    
    [SerializeField] private DigimonBase[] digimons;
    [NonSerialized] private Dictionary<int, DigimonBase> digimonDict;

    protected void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (digimonDict == null)
        {
            digimonDict = new Dictionary<int, DigimonBase>(digimons.Select(digimon => new KeyValuePair<int, DigimonBase>(digimon.DigimonNum, digimon)));
        }
    }

    public DigimonBase this[int digimonNum] => digimonDict[digimonNum];
    public int DigimonTableLength => digimons.Length;
}
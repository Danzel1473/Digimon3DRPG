using System;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [SerializeField] public string uuid = Guid.NewGuid().ToString();
    [SerializeField] public PlayerData npcData;
    [SerializeField] public NPCType npcType;
    [SerializeField] private string script;
    public bool hasBattled = false;
 
    public void Start()
    {
        foreach(Digimon digimon in npcData.partyData.Digimons)
        {
            digimon.Initialize();
        }
    }
}

public enum NPCType
{
    Enemy,
    Boss,
    Neutral
}
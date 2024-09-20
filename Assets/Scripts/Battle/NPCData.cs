using System;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    private int battleCount;
    [SerializeField] private int canBattleCount;
    [SerializeField] public PlayerData npcData;
    [SerializeField] public NPCType npcType;
    [SerializeField] private string script;

    public bool CanBattle()
    {
        if(canBattleCount == 0) return true;
        return canBattleCount <= battleCount;
    }

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
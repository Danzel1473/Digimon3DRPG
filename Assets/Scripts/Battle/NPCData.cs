using System;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    private int battleCount;
    [SerializeField] private int canBattleCount;
    [SerializeField] public NPCType npcType;
    [SerializeField] private PlayerData npcData;
    [SerializeField] private string script;

    public bool CanBattle()
    {
        if(canBattleCount == 0) return true;
        return canBattleCount <= battleCount;
    }
}

public enum NPCType
{
    Enemy,
    Boss,
    Neutral
}
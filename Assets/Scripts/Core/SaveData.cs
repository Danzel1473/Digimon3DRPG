using System;
using System.Collections.Generic;
using System.Numerics;

[Serializable]
public class SaveData
{
    public PlayerData playerData;
    public List<NPCData> npcDataList = new List<NPCData>();
    public Vector3 playerPosition;
}
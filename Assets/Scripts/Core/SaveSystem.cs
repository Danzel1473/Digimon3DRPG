using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/savefile.json";

    public static void SaveGame(PlayerData playerData, List<NPCData> npcDataList)
    {
        SaveData saveData = new SaveData
        {
            playerData = playerData,
            npcDataList = npcDataList
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("게임이 저장되었습니다.");
    }
}

public class LoadSystem
{
    private static string savePath = Application.persistentDataPath + "/savefile.json";

    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            // 플레이어 데이터 복원
            GameManager.Instance.playerData = saveData.playerData;

            // NPC 데이터 복원
            foreach (var npcData in saveData.npcDataList)
            {
                // NPCData existingNPC = GameManager.Instance.GetNPCByUUID(npcData.uuid);
                // if (existingNPC != null)
                // {
                //     // 기존 NPC 정보 업데이트
                //     existingNPC.hasBattled = npcData.hasBattled;
                // }
            }

            Debug.Log("게임이 로드되었습니다.");
            return saveData;
        }
        else
        {
            Debug.LogWarning("세이브 파일을 찾을 수 없습니다.");
            return null;
        }
    }
}
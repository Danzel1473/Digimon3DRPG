using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame()
    {
        PlayerData playerData = GameManager.Instance.playerData;
        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;

        SaveData saveData = new SaveData
        {
            playerData = playerData,
            playerPosition = playerPosition,
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("게임이 저장되었습니다.");
    }

    public static void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            while(!GameManager.Instance) continue;

            GameManager.Instance.playerData = saveData.playerData;
            GameObject.FindWithTag("Player").transform.position = saveData.playerPosition;
        }
        else
        {
            Debug.LogWarning("세이브 파일을 찾을 수 없습니다.");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        //savePath = Application.persistentDataPath + "/gamedata.json";
        savePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
    }

    public void SaveGame(GameData gameData)
    {
        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogError("Save path is not set.");
            return;
        }
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(savePath, json);
    }

    public GameData LoadGame()
    {

        // 파일이 존재하는지 확인
        if (File.Exists(savePath))
        {
            // 파일에서 JSON 데이터 읽기
            string json = File.ReadAllText(savePath);

            // JSON 데이터를 GameData 객체로 역직렬화
            return JsonUtility.FromJson<GameData>(json);
            Debug.Log("존재");
        }

        // 파일이 존재하지 않으면 null 반환
        Debug.Log("존재X");
        return null;
    }

}

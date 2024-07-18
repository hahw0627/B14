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

        // ������ �����ϴ��� Ȯ��
        if (File.Exists(savePath))
        {
            // ���Ͽ��� JSON ������ �б�
            string json = File.ReadAllText(savePath);

            // JSON �����͸� GameData ��ü�� ������ȭ
            return JsonUtility.FromJson<GameData>(json);
            Debug.Log("����");
        }

        // ������ �������� ������ null ��ȯ
        Debug.Log("����X");
        return null;
    }

}

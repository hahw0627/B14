using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string playerSavePath;
    public PlayerDataSO playerDataSO;


    private void Awake()
    {
         playerSavePath = Application.persistentDataPath + "/playerSOdata.json";
      
    }
    
    public void SaveSOData()
    {
        string playerJson = JsonUtility.ToJson(playerDataSO,true);
        File.WriteAllText(playerSavePath, playerJson);
        
    }

    public bool ExistJson()
    {
        if (File.Exists(playerSavePath))
        return true;
        else return false;
    }
 
    public void LoadSOData()
    {
        if (File.Exists(_playerSavePath))
        {
            var playerJson = File.ReadAllText(_playerSavePath);
            JsonUtility.FromJsonOverwrite(playerJson, PlayerDataSO);
            Debug.Log("<color=#00ff00>PlayerDataSO loaded from JSON.</color>");
        }
        else
        {
            Debug.LogWarning("<color=yellow>Player data JSON file not found.</color>");
        }

      
    }
}

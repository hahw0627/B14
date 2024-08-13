using System.Collections;
using System.Collections.Generic;
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

        if (File.Exists(playerSavePath))
        {
            string playerJson = File.ReadAllText(playerSavePath);
            JsonUtility.FromJsonOverwrite(playerJson, playerDataSO);
            Debug.Log("PlayerDataSO loaded from JSON.");
        }
        else
        {
            Debug.LogWarning("Player data JSON file not found.");
        }

      
    }
}

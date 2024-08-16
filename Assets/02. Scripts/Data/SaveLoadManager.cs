using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string _playerSavePath;
    private string _stageSavePath;

    [NonSerialized]
    public string StatSavePath;

    public PlayerDataSO PlayerDataSO;
    public StageDataSO StageDataSO;
    public StatDataSO StatDataSO;

    protected override void Awake()
    {
        base.Awake();
        _playerSavePath = Application.persistentDataPath + "/playerSOdata.json";
        _stageSavePath = Application.persistentDataPath + "/stageSOdata.json";
        StatSavePath = Application.persistentDataPath + "/statSOdata.json";
    }

    public void SaveSOData()
    {
        var playerJson = JsonUtility.ToJson(PlayerDataSO, true);
        File.WriteAllText(_playerSavePath, playerJson);

        var stageJson = JsonUtility.ToJson(StageDataSO, true);
        File.WriteAllText(_stageSavePath, stageJson);

        var statJson = JsonUtility.ToJson(StatDataSO, true);
        File.WriteAllText(StatSavePath, statJson);
    }

    public static bool ExistJson(string path)
    {
        return File.Exists(path);
    }

    public void LoadSOData()
    {
        if (File.Exists(_playerSavePath))
        {
            var playerJson = File.ReadAllText(_playerSavePath);
            JsonUtility.FromJsonOverwrite(playerJson, PlayerDataSO);
        }

        if (File.Exists(_stageSavePath))
        {
            var stageJson = File.ReadAllText(_stageSavePath);
            JsonUtility.FromJsonOverwrite(stageJson, StageDataSO);
        }

        if (!File.Exists(StatSavePath)) return;
        var statJson = File.ReadAllText(StatSavePath);
        JsonUtility.FromJsonOverwrite(statJson, StatDataSO);

        //Debug.Log("<color=#00ff00>PlayerDataSO loaded from JSON.</color>");
        //Debug.LogWarning("<color=yellow>Player data JSON file not found.</color>");
    }
}
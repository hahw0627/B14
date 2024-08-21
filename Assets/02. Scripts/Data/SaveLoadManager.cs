using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string _playerSavePath;
    private string _stageSavePath;
    private string _saveDataPath;

    [NonSerialized]
    public string StatSavePath;



    public PlayerDataSO PlayerDataSO;
    public StageDataSO StageDataSO;
    public StatDataSO StatDataSO;
    public SaveDataSO saveDataSO;

    protected override void Awake()
    {
        base.Awake();
        _playerSavePath = Application.persistentDataPath + "/playerSOdata.json";
        _stageSavePath = Application.persistentDataPath + "/stageSOdata.json";
        StatSavePath = Application.persistentDataPath + "/statSOdata.json";
        _saveDataPath = Application.persistentDataPath + "SaveDataSO.json";
    }

    public void SaveSOData()
    {
        var playerJson = JsonUtility.ToJson(PlayerDataSO, true);
        File.WriteAllText(_playerSavePath, playerJson);

        var stageJson = JsonUtility.ToJson(StageDataSO, true);
        File.WriteAllText(_stageSavePath, stageJson);

        var statJson = JsonUtility.ToJson(StatDataSO, true);
        File.WriteAllText(StatSavePath, statJson);
        var dataJson = JsonUtility.ToJson(saveDataSO, true);
        File.WriteAllText(_saveDataPath, dataJson);
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

        if (File.Exists(_saveDataPath))
        {
            var dataJson = File.ReadAllText(_saveDataPath);
            JsonUtility.FromJsonOverwrite(dataJson, StageDataSO);
        }

    }
}
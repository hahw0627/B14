using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTest : MonoBehaviour
{
    public GameData gameData;
    private SaveLoadManager saveLoadManager;

    private void Start()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        Init();
        LoadGame();
    }
    void Init()
    {
        if (gameData == null)
        {
            // ������ �������� �ʴ� ��� �⺻ �����͸� �����մϴ�.
            gameData = new GameData
            {

                characterData = new CharacterData
                {
                    name = "New Player",
                    level = 1,
                    experience = 0,
                    stats = new Stats { health = 100, attack = 10, defense = 5 },
                    skills = new List<Skill>(),
                    equipments = new List<Equipment>(),
                    pets = new List<Pet>(),
                    companions = new List<Companion>()
                },
                clearedStages = new List<StageData>(),
                currencyData = new CurrencyData { gold = 0, dia = 0 }
            };
        }
    }
    public void LoadGame()
    {
        gameData = saveLoadManager.LoadGame();

    }

    public void SaveGame()
    {
        saveLoadManager.SaveGame(gameData);
        // ���� ���� �� ������ ����
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnApplicationQuit()
    {
        SaveGame();
        
    }
}

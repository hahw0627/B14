using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataTest : MonoBehaviour
{
    [SerializeField]
    GameData gameData;
    private SaveLoadManager saveLoadManager;
    PlayerDataSO playerDataSO;
    private void Start()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        
        
        //LoadGame();
    }
    private void Update()
    {

    
        if (Input.GetKeyDown(KeyCode.S))
        {
            TempCsv tempCsv = FindAnyObjectByType<TempCsv>();
            gameData.characterData.equipmentsData.Add(tempCsv.GetEquipmentByCode("N001"));

        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            saveLoadManager.SaveSOData();
            Debug.Log("����Ϸ�");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            saveLoadManager.LoadSOData();
            Debug.Log("�ҷ���");
        }


    }

    void IninData()
    {
        string path = "ScriptableObjects/PlayerDataSO";

        if (playerDataSO == null)
            playerDataSO = Resources.Load<PlayerDataSO>(path);

        gameData = new GameData();
        gameData.characterData = new CharacterData();
        gameData.characterData.stats = new Stats();
        gameData.currencyData = new CurrencyData();
       
        gameData.characterData.level = 1;
        gameData.characterData.experience = 0;
        gameData.characterData.name = playerDataSO.PlayerName;
        gameData.characterData.stats.health = playerDataSO.Hp;
        gameData.characterData.stats.attack = playerDataSO.Damage;
        gameData.characterData.stats.defense = playerDataSO.Def;
        gameData.characterData.stats.totalPower = playerDataSO.TotalPower;
        gameData.characterData.stats.attackSpeed = playerDataSO.AttackSpeed;
        gameData.characterData.stats.hpRecovery = playerDataSO.HpRecovery;

        gameData.currencyData.gold = playerDataSO.Gold;
        gameData.currencyData.dia = playerDataSO.Gem;

        
        //equipment
        //�׳� �ʿ��Ҷ��� �ڵ�� �ҷ��͵� ��������Ѵ�. 
        //�ڵ�� �ǹ̸� ������ ������ �׳� ���ڷ� �޴°� �� ������, B S  �̷������� Ÿ���� �����ؼ�
        //split �ؼ� ����Ҽ����ִ� Ÿ����,

        //gameData.characterData.equipmentsData.Add(tempCsv.GetEquipmentByCode("������")); 
    }


    //public void LoadGame()
    //{
    //    gameData = saveLoadManager.LoadGame();
    //    if (gameData == null)
    //    {
    //        IninData();
    //        Debug.Log("��� �ε� ����");

    //    }
    //    else
    //    {         
    //        Debug.Log("�ִ��� �ε���");
    //    }
       
    //}

    //public void SaveGame()
    //{
    //    saveLoadManager.SaveGame(gameData);
    //    // ���� ���� �� ������ ����
    //}
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
       // SaveGame();
    }
}

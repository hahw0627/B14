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
            Debug.Log("저장완료");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            saveLoadManager.LoadSOData();
            Debug.Log("불러옴");
        }


    }

    void IninData()
    {
        string path = "ScripableObjects/PlayerDataSO";

        if (playerDataSO == null)
            playerDataSO = Resources.Load<PlayerDataSO>(path);

        gameData = new GameData();
        gameData.characterData = new CharacterData();
        gameData.characterData.stats = new Stats();
        gameData.currencyData = new CurrencyData();
       
        gameData.characterData.level = 1;
        gameData.characterData.experience = 0;
        gameData.characterData.name = playerDataSO.playerName;
        gameData.characterData.stats.health = playerDataSO.Hp;
        gameData.characterData.stats.attack = playerDataSO.Damage;
        gameData.characterData.stats.defense = playerDataSO.Def;
        gameData.characterData.stats.totalPower = playerDataSO.TotalPower;
        gameData.characterData.stats.attackSpeed = playerDataSO.AttackSpeed;
        gameData.characterData.stats.hpRecovery = playerDataSO.HpRecovery;

        gameData.currencyData.gold = playerDataSO.Gold;
        gameData.currencyData.dia = playerDataSO.Diamond;

        
        //equipment
        //그냥 필요할때만 코드로 불러와도 상관없다한다. 
        //코드는 의미를 가지지 않으면 그냥 숫자로 받는게 더 가볍다, B S  이런식으로 타입을 지정해서
        //split 해서 사용할수도있다 타입을,

        //gameData.characterData.equipmentsData.Add(tempCsv.GetEquipmentByCode("ㅁㄴㅇ")); 
    }


    //public void LoadGame()
    //{
    //    gameData = saveLoadManager.LoadGame();
    //    if (gameData == null)
    //    {
    //        IninData();
    //        Debug.Log("없어서 로드 못함");

    //    }
    //    else
    //    {         
    //        Debug.Log("있던거 로드함");
    //    }
       
    //}

    //public void SaveGame()
    //{
    //    saveLoadManager.SaveGame(gameData);
    //    // 게임 종료 시 데이터 저장
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

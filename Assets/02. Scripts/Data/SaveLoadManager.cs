using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private string playerSavePath;
    private string skillsSavePath;

    public PlayerDataSO playerDataSO;
    public List<SkillDataSO> skillsDataSO = new List<SkillDataSO>();

    private void Awake()
    {
        playerSavePath = Application.persistentDataPath + "/playerSOdata.json";
        skillsSavePath = Application.persistentDataPath + "/skillsSOdata.json";

    }

    public void ReceiveSOData()
    {
        //playerDataSO = FindObjectOfType<Player>().playerData;
        SkillManager skillManager = FindObjectOfType<SkillManager>();
        if (skillManager != null)
        {
            if (skillManager.equippedSkills == null)
                return;

            foreach (var skill in skillManager.equippedSkills)
            {
                skillsDataSO.Add(skill);
            }
        }
    }

    public void SaveSOData()
    {
        ReceiveSOData();
        string playerJson = JsonUtility.ToJson(playerDataSO,true);
        SkillsDataWrapper wrapper = new SkillsDataWrapper
        {
            skills = skillsDataSO
        };
        string skillsJson = JsonUtility.ToJson(wrapper, true);
        

        Debug.Log("PlayerJSON: " + playerJson);
        Debug.Log("SkillsJSON: " + skillsJson);
       
        File.WriteAllText(playerSavePath, playerJson);
        File.WriteAllText(skillsSavePath, skillsJson);

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



        if (File.Exists(skillsSavePath))
        {
            string skillsJson = File.ReadAllText(skillsSavePath);
            // JSON 문자열을 역직렬화하여 List<SkillDataSO>를 갱신합니다
            SkillsDataWrapper skillsWrapper = JsonUtility.FromJson<SkillsDataWrapper>(skillsJson);
            skillsDataSO = skillsWrapper.skills;
            Debug.Log("SkillsDataSO loaded from JSON.");
        }
        else
        {
            Debug.LogWarning("Skills data JSON file not found.");
        }
    }
    [System.Serializable]
    private class SkillsDataWrapper
    {
        //기존 jsonutility 는 복잡한 구조를 다루는데 한계가있어서 래퍼 클래스로 변환해서 전달해야한다
        //이게 싫으면 Newtonsoft.json은 복잡한 구조도 지원한다.
        public List<SkillDataSO> skills = new List<SkillDataSO>();
    }


    public void SaveGame(GameData gameData)
    {
        if (string.IsNullOrEmpty(playerSavePath))
        {
            Debug.LogError("Save path is not set.");
            return;
        }
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(playerSavePath, json);
    }

    public GameData LoadGame()
    {

        // 파일이 존재하는지 확인
        if (File.Exists(playerSavePath))
        {
            // 파일에서 JSON 데이터 읽기
            string json = File.ReadAllText(playerSavePath);
           Debug.Log(json);
            // JSON 데이터를 GameData 객체로 역직렬화
            return JsonUtility.FromJson<GameData>(json);
            
        }

        // 파일이 존재하지 않으면 null 반환
        Debug.Log("존재X");
        return null;
    }

}

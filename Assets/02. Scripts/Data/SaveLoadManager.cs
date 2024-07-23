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
            // JSON ���ڿ��� ������ȭ�Ͽ� List<SkillDataSO>�� �����մϴ�
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
        //���� jsonutility �� ������ ������ �ٷ�µ� �Ѱ谡�־ ���� Ŭ������ ��ȯ�ؼ� �����ؾ��Ѵ�
        //�̰� ������ Newtonsoft.json�� ������ ������ �����Ѵ�.
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

        // ������ �����ϴ��� Ȯ��
        if (File.Exists(playerSavePath))
        {
            // ���Ͽ��� JSON ������ �б�
            string json = File.ReadAllText(playerSavePath);
           Debug.Log(json);
            // JSON �����͸� GameData ��ü�� ������ȭ
            return JsonUtility.FromJson<GameData>(json);
            
        }

        // ������ �������� ������ null ��ȯ
        Debug.Log("����X");
        return null;
    }

}

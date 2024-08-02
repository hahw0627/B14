using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public PlayerDataSO          playerDataSO;
    public List<EquipmentDataSO> weaponEquipmentDataSO;
    public List<EquipmentDataSO> armorEquipmentDataSO;
    public List<SkillDataSO>     allSkillsDataSO;


    public void Start()
    {
        if (playerDataSO == null)
        {
            playerDataSO = Resources.Load<PlayerDataSO>("ScripableObjects/" + "PlayerDataSO");
        }
    }
   
    //addRange�� �ѹ��� �ټ��� ���Ҹ� �߰��ϴµ� �������� ����. 
    //���� �������� ���ο� ����Ʈ�� �߰��ؼ� �÷��ִ� �Լ�.
    public void FillAllSkillsData(List<SkillDataSO> _skillsDataSO)
    {
        allSkillsDataSO.AddRange(_skillsDataSO);
    }
    public void FillAllWeaponsData(List<EquipmentDataSO> _weaponEquipmentDataSO)
    {
        weaponEquipmentDataSO.AddRange(_weaponEquipmentDataSO);
    }
    public void FillAllarmorData(List<EquipmentDataSO> _armorEquipmentDataSO)
    {
        armorEquipmentDataSO.AddRange(_armorEquipmentDataSO);
    }

    public void AddGold(int amount)
    {
        playerDataSO.Gold += amount;
        Debug.Log($"�÷��̾��� ���� ��� : {playerDataSO.Gold}");
        UIManager.Instance.UpdateCurrencyUI();
    }
}

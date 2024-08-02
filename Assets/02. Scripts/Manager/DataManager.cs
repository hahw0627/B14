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
   
    //addRange는 한번에 다수의 원소를 추가하는데 마지막에 들어간다. 
    //기존 데이터의 새로운 리스트를 추가해서 늘려주는 함수.
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
        Debug.Log($"플레이어의 현재 골드 : {playerDataSO.Gold}");
        UIManager.Instance.UpdateCurrencyUI();
    }
}

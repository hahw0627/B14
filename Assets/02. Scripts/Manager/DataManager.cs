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

    //list에 저장된 모든 스킬 so 데이터를 새로 만들 skill list에 넣어주는 함수
    public void FillAllSkillsData(List<SkillDataSO> _skillsDataSO)
    {
        _skillsDataSO.AddRange(allSkillsDataSO);
    }
    public void FillAllWeaponsData(List<EquipmentDataSO> _weaponEquipmentDataSO)
    {
        _weaponEquipmentDataSO.AddRange(weaponEquipmentDataSO);
    }
    public void FillAllarmorData(List<EquipmentDataSO> _armorEquipmentDataSO)
    {
        _armorEquipmentDataSO.AddRange(armorEquipmentDataSO);
    }

}

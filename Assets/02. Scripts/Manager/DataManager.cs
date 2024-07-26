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


    //list�� ����� ��� ��ų so �����͸� ���� ���� skill list�� �־��ִ� �Լ�
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

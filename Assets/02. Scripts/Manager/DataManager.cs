using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManager : Singleton<DataManager>
{
    [FormerlySerializedAs("playerDataSO")]
    public PlayerDataSO PlayerDataSo;

    [FormerlySerializedAs("weaponEquipmentDataSO")]
    public List<EquipmentDataSO> WeaponEquipmentDataSo;

    [FormerlySerializedAs("armorEquipmentDataSO")]
    public List<EquipmentDataSO> ArmorEquipmentDataSo;

    [FormerlySerializedAs("allSkillsDataSO")]
    public List<SkillDataSO> AllSkillsDataSo;


    public void Start()
    {
        if (PlayerDataSo == null)
        {
            PlayerDataSo = Resources.Load<PlayerDataSO>("ScriptableObjects/" + "PlayerDataSO");
        }

        DontDestroyOnLoad(gameObject);
    }


    public void FillAllSkillsData(List<SkillDataSO> skillsDataSo)
    {
        AllSkillsDataSo.AddRange(skillsDataSo);
    }

    public void FillAllWeaponsData(List<EquipmentDataSO> weaponEquipmentDataSo)
    {
        WeaponEquipmentDataSo.AddRange(weaponEquipmentDataSo);
    }

    public void FillAllarmorData(List<EquipmentDataSO> armorEquipmentDataSo)
    {
        ArmorEquipmentDataSo.AddRange(armorEquipmentDataSo);
    }

    public void AddGold(int amount)
    {
        PlayerDataSo.Gold += amount;
        Debug.Log($"<color=yellow>방치 보상 획득 후 골드 : {PlayerDataSo.Gold}</color>");
        UIManager.Instance.UpdateCurrencyUI();
    }

    public void AddGem(int amount)
    {
        PlayerDataSo.Gem += amount;
        Debug.Log($"<color=#00FF22>젬 증가: {PlayerDataSo.Gem}</color>");
        UIManager.Instance.UpdateCurrencyUI();
    }
}
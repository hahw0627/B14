using System.Collections.Generic;
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
   
    //addRangeï¿½ï¿½ ï¿½Ñ¹ï¿½ï¿½ï¿½ ï¿½Ù¼ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Ò¸ï¿½ ï¿½ß°ï¿½ï¿½Ï´Âµï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½î°£ï¿½ï¿? 
    //ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Î¿ï¿½ ï¿½ï¿½ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½ß°ï¿½ï¿½Ø¼ï¿½ ï¿½Ã·ï¿½ï¿½Ö´ï¿½ ï¿½Ô¼ï¿½.
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
        Debug.Log($"ÇÃ·¹ÀÌ¾îÀÇ ÇöÀç °ñµå : {playerDataSO.Gold}");
        UIManager.Instance.UpdateCurrencyUI();
    }
}

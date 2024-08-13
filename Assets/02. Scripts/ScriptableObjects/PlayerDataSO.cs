using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    
    public List<SkillDataSO> skills;    //ï¿½Îºï¿½ï¿½ä¸® Ã³ï¿½ï¿½ ï¿½ï¿½ï¿½â¿¡ ï¿½ï¿½Å³ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½î°¡ï¿½Ôµï¿?
    public List<CompanionDataSO> companions;        //ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½

    public List<EquipmentDataSO> weapons; //ï¿½ï¿½ï¿½ï¿½ 
    public List<EquipmentDataSO> armors;  //ï¿½ï¿½î±?
    //ï¿½ß°ï¿½

    public string playerName;
    // ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½Ì¸ï¿½
    // ï¿½Ô½ï¿½Æ® ï¿½Î±ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ã·ï¿½ï¿½ï¿½ï¿½Ï¸ï¿½ Guest-Login ï¿½ï¿½ï¿½ï¿½ Ç¥ï¿½ï¿½ï¿½Ï¸ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    // ï¿½Ò¼ï¿½ ï¿½Î±ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ã·ï¿½ï¿½ï¿½ï¿½Ï¸ï¿½ ?
    public int Gold;                // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½È­ (ï¿½ï¿½È­ï¿½ï¿½)
    public int Gem;
    public int Diamond;             // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½È­ (ï¿½Ì±ï¿½ï¿?
  
    public int TotalPower;          // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
    public int Damage;              // ï¿½ï¿½ï¿½Ý·ï¿½
    public int Def;                 // ï¿½ï¿½ï¿½ï¿½
    public float AttackSpeed;       // ï¿½ï¿½ï¿½Ý¼Óµï¿½
    
    public int Hp;                  // Ã¼ï¿½ï¿½
    public int HpRecovery;          // Ã¼ï¿½ï¿½È¸ï¿½ï¿½ (1ï¿½Ê´ï¿½ È¸ï¿½ï¿½)

    // Ä¡¸íÅ¸´Â ÃÖ´ë °­È­ ·¹º§À» 500À¸·Î?
    public float CriticalPer;           // Ä¡¸íÅ¸ È®·ü 1·¹º§ ½ÃÀÛ ( °­È­¸¶´Ù 0.2 Áõ°¡ÇÏ¸é µÉ µíÇÔ )
    public float CriticalMultiplier;    // Ä¡¸íÅ¸ ¹èÀ² 1·¹º§ ½ÃÀÛ ( 0·¹º§ 150ÆÛ¼¾Æ® µ¥¹ÌÁö ½ÃÀÛ + ·¹º§´ç 0.3%Áõ°¡? )


    public EquipmentDataSO currentWeaponEquip;
    public EquipmentDataSO currentArmorEquip;

    

}


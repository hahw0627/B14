using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    
    public List<SkillDataSO> skills;
    public List<CompanionDataSO> companions;     

    public List<EquipmentDataSO> weapons; 
    public List<EquipmentDataSO> armors;  


    public string playerName;
   
    public int Gold;                
    public int Gem;
    public int Diamond;           
  
    public int TotalPower;          
    public int Damage;            
    public int Def;                
    public float AttackSpeed;       
    
    public int Hp;                 
    public int HpRecovery;         

    public float CriticalPer;       
    public float CriticalDamage;    

    public EquipmentDataSO currentWeaponEquip;
    public EquipmentDataSO currentArmorEquip;


    public int attackCost;
    public int hpCost;
    public int recoverHpCost;
    public int attackSpeedCost;
    public int criticalPercentCost;
    public int criticalDamageCost;
}


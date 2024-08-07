using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    public enum GachaRarity
    {
        Normal,
        Rare,
        //추가 변경
    }
    public enum EquipmentType
    {
        Weapon,
        Armor,
        //추가
    }
    public enum EquipmentGrade
    {
        Common,
        Uncommon,
        Rare,
        //추가
    }

    public enum SkillType
    {
        AttackBuff,
        HealBuff,
        Projectile,
        AreaOfEffect
    }
    public enum SkillRarity
    {
        Normal,
        Rare,
        Unique,
        Epic,
        Legendary
    }

}

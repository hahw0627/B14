using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
[CreateAssetMenu(fileName = "SkillDataSO", menuName = "ScriptableObjects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public int level;
    public SkillRarity rarity;
    public string description;
    public float duration;
    public float cooldown;
    public int damage;
    public GameObject effectPrefab;
    public SkillType skillType;
    public float projectileSpeed; // 투사체 스킬용
    public float aoeRadius; // 범위 스킬용
    public int buffAmount; // 공격력 올라가는 수치 or 회복량
}

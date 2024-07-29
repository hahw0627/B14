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
    public float projectileSpeed; // ����ü ��ų��
    public float aoeRadius; // ���� ��ų��
    public int buffAmount; // ���ݷ� �ö󰡴� ��ġ or ȸ����
}

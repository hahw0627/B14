using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillDataSO", menuName = "ScriptableObjects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public int level;
    public Define.SkillRarity rarity;
    public string description;
    public float duration;
    public float cooldown;
    public int damage;
    public GameObject effectPrefab;
    public Define.SkillType skillType;
    public float projectileSpeed; // 투사체 스킬용
    public float aoeRadius; // 범위 스킬용
    public int buffAmount; // 공격력 올라가는 수치 or 회복량
    public int count;
}

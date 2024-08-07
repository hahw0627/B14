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
    public float projectileSpeed; // ����ü ��ų��
    public float aoeRadius; // ���� ��ų��
    public int buffAmount; // ���ݷ� �ö󰡴� ��ġ or ȸ����
    public int count;
}

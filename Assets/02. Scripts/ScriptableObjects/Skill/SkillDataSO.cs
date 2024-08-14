using UnityEngine;
using UnityEngine.Serialization;

//[System.Serializable]
[CreateAssetMenu(fileName = "SkillDataSO", menuName = "ScriptableObjects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    [FormerlySerializedAs("skillName")]
    public string SkillName;

    [FormerlySerializedAs("icon")]
    public Sprite Icon;

    [FormerlySerializedAs("level")]
    public int Level;

    [FormerlySerializedAs("rarity")]
    public Define.SkillRarity Rarity;

    [FormerlySerializedAs("description")]
    public string Description;

    [FormerlySerializedAs("duration")]
    public float Duration;

    [FormerlySerializedAs("cooldown")]
    public float Cooldown;

    [FormerlySerializedAs("damage")]
    public int Damage;

    [FormerlySerializedAs("effectPrefab")]
    public GameObject EffectPrefab;

    [FormerlySerializedAs("skillType")]
    public Define.SkillType SkillType;

    [FormerlySerializedAs("projectileSpeed")]
    public float ProjectileSpeed; // ����ü ��ų��

    [FormerlySerializedAs("aoeRadius")]
    public float AoeRadius; // ���� ��ų��

    [FormerlySerializedAs("buffAmount")]
    public int BuffAmount; // ���ݷ� �ö󰡴� ��ġ or ȸ����

    [FormerlySerializedAs("count")]
    public int Count;

    [FormerlySerializedAs("isUnlocked")]
    public bool IsUnlocked;
}
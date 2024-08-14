using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
[CreateAssetMenu(fileName = "NewCompanionData", menuName = "ScriptableObjects/CompanionDataSO", order = 3)]
public class CompanionDataSO : ScriptableObject
{
    [FormerlySerializedAs("companionName")]
    public string CompanionName;

    [FormerlySerializedAs("icon")]
    public Sprite Icon;

    [FormerlySerializedAs("level")]
    public int Level;

    [FormerlySerializedAs("rarity")]
    public Define.SkillRarity Rarity;

    [FormerlySerializedAs("description")]
    public string Description;

    [FormerlySerializedAs("damage")]
    public int Damage;

    [FormerlySerializedAs("attackSpeed")]
    public float AttackSpeed;

    [FormerlySerializedAs("count")]
    public int Count;

    [FormerlySerializedAs("isEquipped")]
    public bool IsEquipped;
}
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewCompanionData", menuName = "ScriptableObjects/CompanionDataSO", order = 3)]
public class CompanionDataSO : ScriptableObject
{
    public string companionName;
    public Sprite icon;
    public int level;
    public Define.SkillRarity rarity;
    public string description;
    public int damage;
    public float attackSpeed;
    public int count;
    public bool isEquipped;
}

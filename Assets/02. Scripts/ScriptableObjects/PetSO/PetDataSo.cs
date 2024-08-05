using UnityEngine;

[CreateAssetMenu(fileName = "NewPetData", menuName = "ScriptableObjects/PetDataSO", order = 3)]
public class PetDataSO : ScriptableObject
{
    public string petName;
    public Sprite icon;
    public int level;
    public Define.SkillRarity rarity;
    public string description;
    public int damage;
    public float attackSpeed;
    public int count;

    // 펫 넘버를 비교하여 보유 효과, 장착 효과 능력치 증가 기능? 어떻게?
}

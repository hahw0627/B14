using UnityEngine;
using UnityEngine.UI;

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
    public bool isEquipped;

    // �� �ѹ��� ���Ͽ� ���� ȿ��, ���� ȿ�� �ɷ�ġ ���� ���? ���?
}

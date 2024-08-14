using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
[CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "ScriptableObjects/EquipmentDataSO", order = 6)]
public class EquipmentDataSO : ScriptableObject
{
    [FormerlySerializedAs("equipmentType")]
    public Define.EquipmentType EquipmentType;

    [FormerlySerializedAs("equipmentGrade")]
    public Define.EquipmentGrade EquipmentGrade;

    [FormerlySerializedAs("gachaRarity")]
    public Define.GachaRarity GachaRarity;

    [FormerlySerializedAs("itemName")]
    public string ItemName;

    [FormerlySerializedAs("description")]
    public string Description;

    [FormerlySerializedAs("sprite")]
    public Sprite Sprite;

    [FormerlySerializedAs("atk")]
    public float Atk; //���ݷ�

    [FormerlySerializedAs("def")]
    public float Def; //����

    [FormerlySerializedAs("attackSpeed")]
    public float AttackSpeed; //���ݼӵ�.

    [FormerlySerializedAs("currentLevel")]
    public int CurrentLevel;

    public int MaxLevel;

    [FormerlySerializedAs("baseGoldCost")]
    public int BaseGoldCost; // 1���� ���׷��̵忡 �ʿ��� ��差

    [FormerlySerializedAs("attackIncreasePerLevel")]
    public float AttackIncreasePerLevel; //������ ���ݷ� ��·�

    [FormerlySerializedAs("defIncreasePerLevel")]
    public float DefIncreasePerLevel; //������ ���� ��·�

    [FormerlySerializedAs("count")]
    public int Count;

    //�߰�
    public void EnhanceItem(Define.EquipmentType equipType)
    {
        switch (equipType)
        {
            case Define.EquipmentType.Weapon:
                Enhance(Define.EquipmentType.Weapon);
                break;
            case Define.EquipmentType.Armor:
                Enhance(Define.EquipmentType.Armor);
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(equipType), equipType, null);
        }
    }

    private void Enhance(Define.EquipmentType equipType)
    {
        if (EquipmentType == Define.EquipmentType.Weapon)
        {
            if (CurrentLevel < MaxLevel)
            {
                var goldRequired = CalculateGoldRequired(CurrentLevel);

                if (DataManager.Instance.PlayerDataSo.Gold >= goldRequired)
                {
                    DataManager.Instance.PlayerDataSo.Gold -= goldRequired;
                    CurrentLevel++;
                    Atk += AttackIncreasePerLevel;

                    Debug.Log(
                        $"������ '{ItemName}'��(��) {CurrentLevel}������ ��ȭ�Ǿ����ϴ�. ���ݷ�: {Atk}, ���� ���: {DataManager.Instance.PlayerDataSo.Gold}");
                }
                else
                {
                    Debug.Log(
                        $"��尡 �����մϴ�! �ʿ��� ���: {goldRequired}, ���� ���: {DataManager.Instance.PlayerDataSo.Gold}");
                }
            }
            else
            {
                Debug.Log($"{ItemName}��(��) �̹� �ִ� �����Դϴ�.");
            }
        }
        else
        {
            //�� �߰� 
        }
    }

    private int CalculateGoldRequired(int currentLevel)
    {
        //��差�� ������ ���� 1.5�辿 ����
        return BaseGoldCost * (int)Mathf.Pow(1.5f, currentLevel);
    }
}
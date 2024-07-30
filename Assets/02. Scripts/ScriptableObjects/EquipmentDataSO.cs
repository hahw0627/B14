using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "ScriptableObjects/EquipmentDataSO", order = 6)]
public class EquipmentDataSO : ScriptableObject
{

    public Define.EquipmentType equipmentType;
    public Define.EquipmentGrade equipmentGrade;
    public Define.GachaRarity gachaRarity;

    public string itemName;
    public string description;

    public Sprite sprite;

    public float atk; //���ݷ�
    public float def; //����
    public float attackSpeed; //���ݼӵ�.

    public int currentLevel;
    public int MaxLevel;

    public int baseGoldCost; // 1���� ���׷��̵忡 �ʿ��� ��差
    public float attackIncreasePerLevel; //������ ���ݷ� ��·�
    public float defIncreasePerLevel;   //������ ���� ��·�

    //�߰�
    public void EnhanceItem(Define.EquipmentType equipType)
    {
        switch(equipType)
        {
            case Define.EquipmentType.Weapon:
                Enhance(Define.EquipmentType.Weapon);
                break;
            case Define.EquipmentType.Armor:
                Enhance(Define.EquipmentType.Armor);
                break;
        }
    }
    void Enhance(Define.EquipmentType equipType)
    {
        if (equipmentType == Define.EquipmentType.Weapon)
        {
            if (currentLevel < MaxLevel)
            {
                int goldRequired = CalculateGoldRequired(currentLevel);

                if (DataManager.Instance.playerDataSO.Gold >= goldRequired)
                {
                    DataManager.Instance.playerDataSO.Gold -= goldRequired;
                    currentLevel++;
                    atk += attackIncreasePerLevel;

                    Debug.Log($"������ '{itemName}'��(��) {currentLevel}������ ��ȭ�Ǿ����ϴ�. ���ݷ�: {atk}, ���� ���: {DataManager.Instance.playerDataSO.Gold}");
                }
                else
                {
                    Debug.Log($"��尡 �����մϴ�! �ʿ��� ���: {goldRequired}, ���� ���: {DataManager.Instance.playerDataSO.Gold}");
                }
            }
            else
            {
                Debug.Log($"{itemName}��(��) �̹� �ִ� �����Դϴ�.");
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
        return baseGoldCost * (int)Mathf.Pow(1.5f, currentLevel);
    }
}
    

    


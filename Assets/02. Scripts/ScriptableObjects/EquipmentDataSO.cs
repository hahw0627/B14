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

    public float atk; //공격력
    public float def; //방어력
    public float attackSpeed; //공격속도.

    public int currentLevel;
    public int MaxLevel;

    public int baseGoldCost; // 1렙때 업그레이드에 필요한 골드량
    public float attackIncreasePerLevel; //레벨당 공격력 상승량
    public float defIncreasePerLevel;   //레벨당 방어력 상승량

    //추가
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

                    Debug.Log($"아이템 '{itemName}'이(가) {currentLevel}레벨로 강화되었습니다. 공격력: {atk}, 남은 골드: {DataManager.Instance.playerDataSO.Gold}");
                }
                else
                {
                    Debug.Log($"골드가 부족합니다! 필요한 골드: {goldRequired}, 현재 골드: {DataManager.Instance.playerDataSO.Gold}");
                }
            }
            else
            {
                Debug.Log($"{itemName}은(는) 이미 최대 레벨입니다.");
            }
        }
        else
        {
            //방어구 추가 
        }

    }

    private int CalculateGoldRequired(int currentLevel)
    {
        //골드량이 레벨에 따라 1.5배씩 증가
        return baseGoldCost * (int)Mathf.Pow(1.5f, currentLevel);
    }
}
    

    


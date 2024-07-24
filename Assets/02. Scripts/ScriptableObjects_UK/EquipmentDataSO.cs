using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "ScriptableObjects/EquipmentDataSO", order = 6)]
public class EquipmentDataSO : ScriptableObject
{
   public Define.EquipmentType equipmentType;
   public Define.EquipmentGrade equipmentGrade;
   public Define.GachaRarity gachaRarity;
   
   public string itemName;
   public string description;
  
   public Sprite sprite;
  
   public int atk; //���ݷ�
   public int def; //����

    //�߰�


}

using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[Serializable]
public class GameData
{
    public CharacterData characterData;
    public List<StageData> clearedStages; //Ŭ���� �� �������� ���� ����
    public CurrencyData currencyData;
    //�߰�

}

[Serializable]
public class CharacterData
{
    public string name;
    public int level;
    public int experience;

    public Stats stats;
    public List<Skill> skills;
    public List<EquipmentData> equipmentsData;
    //public List<Pet> pets;
    //public List<Companion> companions; //����

 
}

[Serializable]
public class Stats
{
    public int health;
    public int hpRecovery;

    public long attack;
    public int defense;
    public int totalPower;
    public float attackSpeed;
    
    //�߰�
}

[Serializable]
public class Skill
{
    public string skillName;
    public int level;
    public string description;
}


[Serializable]
public class EquipmentData
{
    public string id;                                  //�з��� itemcode
    public Define.GachaRarity gachaRarity;          //���
    public Define.EquipmentType equipmentType;      //��� Ÿ��
    public Define.EquipmentGrade equipmentGrade;    //��� ���

    public string equipmentName;                    //��� �̸�
    public string description;                      //����
    public string spriteName;                       //��������Ʈ �̸�
    public Sprite sprite;
    public int atackPower;                          //���ݷ�
    public int defense;                             //����


}

//[Serializable]
//public class Pet
//{
//    public string petName;
//    public int level;
//    public string description;
//}

//[Serializable]
//public class Companion
//{
//    public string companionName;
//    public int level;
//    public string description;
//}

[Serializable]
public class StageData
{
    public int stageNumber;
    public bool isCleared;
}

[Serializable]
public class CurrencyData
{
    public long Gold;
    public long Gem;
    // �ʿ信 ���� �߰����� ��ȭ
}
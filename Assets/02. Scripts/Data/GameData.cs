using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Animation;
using UnityEngine;

[Serializable]
public class GameData
{
    public CharacterData characterData;
    public List<StageData> clearedStages; //클리어 된 스테이지 정보 저장
    public CurrencyData currencyData;
    //추가

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
    //public List<Companion> companions; //동료

 
}

[Serializable]
public class Stats
{
    public int health;
    public int hpRecovery;

    public int attack;
    public int defense;
    public int totalPower;
    public float attackSpeed;
    
    //추가
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
    public string id;                                  //분류용 itemcode
    public Define.GachaRarity gachaRarity;          //등급
    public Define.EquipmentType equipmentType;      //장비 타입
    public Define.EquipmentGrade equipmentGrade;    //장비 등급

    public string equipmentName;                    //장비 이름
    public string description;                      //설명
    public string spriteName;                       //스프라이트 이름
    public Sprite sprite;
    public int atackPower;                          //공격력
    public int defense;                             //방어력


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
    public int gold;
    public int dia;
    // 필요에 따라 추가적인 재화
}
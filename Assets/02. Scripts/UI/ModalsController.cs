using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModalsController : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Transform content; //아이템을 표시할 부모 트랜스폼
    public GameObject itemPrefab;
 
    private Define.EquipmentType selectedItemType;
    public TextMeshProUGUI currentItemName;
    public Image currentItemImage;

    public Sprite defaultSprite;

    List<GameObject> itemTableObjects;
    List<EquipmentDataSO> playerWeapons;
    public SpriteRenderer weaponSprite;
   
    private void Start()
    {
       playerWeapons = DataManager.Instance.playerDataSO.weapons;
    }
    public void OnWeaponButtonClick()
    {
        selectedItemType = Define.EquipmentType.Weapon;
        title.text = "무기";
        currentItemName.text = DataManager.Instance.playerDataSO.currentWeaponEquip.itemName;
        currentItemImage.sprite = DataManager.Instance.playerDataSO.currentWeaponEquip.sprite;
        UpdateItemList();
    }


    // 방어구 버튼 클릭 시 호출
    public void OnArmorButtonClick()
    {
        selectedItemType = Define.EquipmentType.Armor;
        title.text = "방어구";
        UpdateItemList();
    }
    public void OnEquip()
    {
        if (selectedItemType == Define.EquipmentType.Weapon)
        {

            foreach (var item in playerWeapons)
            {
                if (item.itemName == currentItemName.text)
                {
                    DataManager.Instance.playerDataSO.currentWeaponEquip = item;

                    weaponSprite.sprite = DataManager.Instance.playerDataSO.currentWeaponEquip.sprite;
                   
                }
                else
                {
                    Debug.Log("없어서 장비 장착 못함.");
                    
                }
            }
        }
        else
        {
            //방어구 추가..
        }
    }
    public void OnEnhanceButton()
    {
        if (selectedItemType == Define.EquipmentType.Weapon)
        {

            foreach (var item in playerWeapons)
            {           
                if (item.itemName == currentItemName.text)
                {
                    item.EnhanceItem(selectedItemType);
                    Debug.Log("강화성공");
                    
                }
                else
                {
                    Debug.Log("인벤에 없어서 강화 실패");
                    
                }
            }
        }
        else
        {
            //방어구 추가..
        }
    }

    private void UpdateItemList()
    {
        
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        if (selectedItemType == Define.EquipmentType.Weapon)
        {

            foreach (var item in DataManager.Instance.weaponEquipmentDataSO)
            {

                GameObject newItem = Instantiate(itemPrefab, content);
                newItem.name = item.itemName;
                //itemTableObjects.Add(newItem);

                bool isInPlayerWeapons = playerWeapons.Exists(weapon => weapon.itemName == item.itemName);
                if (isInPlayerWeapons)
                {
                    newItem.GetComponent<Image>().sprite = item.sprite;
                }
                else
                {
                    newItem.GetComponent<Image>().sprite = defaultSprite;
                }
            }
        }

    }

}

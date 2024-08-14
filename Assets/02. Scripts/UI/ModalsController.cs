using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ModalsController : MonoBehaviour
{
    [FormerlySerializedAs("title")]
    public TextMeshProUGUI Title;

    [FormerlySerializedAs("content")]
    public Transform Content;

    [FormerlySerializedAs("itemPrefab")]
    public GameObject ItemPrefab;

    private Define.EquipmentType _selectedItemType;

    [FormerlySerializedAs("currentItemName")]
    public TextMeshProUGUI CurrentItemName;

    [FormerlySerializedAs("currentItemImage")]
    public Image CurrentItemImage;

    [FormerlySerializedAs("defaultSprite")]
    public Sprite DefaultSprite;

    private List<GameObject> _itemTableObjects;
    private List<EquipmentDataSO> _playerWeapons;

    [FormerlySerializedAs("weaponSprite")]
    public SpriteRenderer WeaponSprite;

    private void Start()
    {
        _playerWeapons = DataManager.Instance.PlayerDataSo.Weapons;
    }

    public void OnWeaponButtonClick()
    {
        _selectedItemType = Define.EquipmentType.Weapon;
        Title.text = "무기";
        CurrentItemName.text = DataManager.Instance.PlayerDataSo.CurrentWeaponEquip.ItemName;
        CurrentItemImage.sprite = DataManager.Instance.PlayerDataSo.CurrentWeaponEquip.Sprite;
        UpdateItemList();
    }


    // ���?��ư Ŭ�� �� ȣ��
    public void OnArmorButtonClick()
    {
        _selectedItemType = Define.EquipmentType.Armor;
        Title.text = "방어구";
        UpdateItemList();
    }

    public void OnEquip()
    {
        if (_selectedItemType == Define.EquipmentType.Weapon)
        {
            foreach (var item in _playerWeapons)
            {
                if (item.ItemName == CurrentItemName.text)
                {
                    DataManager.Instance.PlayerDataSo.CurrentWeaponEquip = item;

                    WeaponSprite.sprite = DataManager.Instance.PlayerDataSo.CurrentWeaponEquip.Sprite;
                }
                else
                {
                    Debug.Log("��� ���?���� ����.");
                }
            }
        }
        else
        {
            //���?�߰�..
        }
    }

    public void OnEnhanceButton()
    {
        if (_selectedItemType == Define.EquipmentType.Weapon)
        {
            foreach (var item in _playerWeapons)
            {
                if (item.ItemName == CurrentItemName.text)
                {
                    item.EnhanceItem(_selectedItemType);
                    Debug.Log("��ȭ����");
                }
                else
                {
                    Debug.Log("�κ��� ��� ��ȭ ����");
                }
            }
        }
        else
        {
            //���?�߰�..
        }
    }

    private void UpdateItemList()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        if (_selectedItemType != Define.EquipmentType.Weapon) return;
        foreach (var item in DataManager.Instance.WeaponEquipmentDataSo)
        {
            var newItem = Instantiate(ItemPrefab, Content);
            newItem.name = item.ItemName;
            //itemTableObjects.Add(newItem);

            var isInPlayerWeapons = _playerWeapons.Exists(weapon => weapon.ItemName == item.ItemName);
            newItem.GetComponent<Image>().sprite = isInPlayerWeapons ? item.Sprite : DefaultSprite;
        }
    }
}
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
    [SerializeField]
    private List<EquipmentDataSO> _playerWeapons;

    [FormerlySerializedAs("weaponSprite")]
    public SpriteRenderer WeaponSprite;
    StatUpgrade statUpgrade;


    private void Start()
    {
        statUpgrade = FindAnyObjectByType<StatUpgrade>();
        foreach (var item in DataManager.Instance.PlayerDataSo.Weapons)
        {
            _playerWeapons.Add(item);
        }
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
                    statUpgrade.UpdateUI();
                    Debug.Log($"{item.name}를 장착했습니다");
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

        //이부분 플레이어데이타 so 에 웨폰에 있는걸 넣어줌.
        foreach (var item in DataManager.Instance.PlayerDataSo.Weapons)
        {
            var newItem = Instantiate(ItemPrefab, Content);
            newItem.name = item.ItemName;
            // _itemTableObjects.Add(newItem);
            var isInPlayerWeapons = _playerWeapons.Exists(weapon => weapon.ItemName == item.ItemName);
            newItem.GetComponent<Image>().sprite = isInPlayerWeapons ? item.Sprite : DefaultSprite;
        }
    }
}
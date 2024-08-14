using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaScroll : MonoBehaviour
{
    public GameObject back;
    public GameObject front;
    public Button OpenButton;
    public Image icon;

    [Header("rarity")]
    public GameObject normal;
    public GameObject rare;
    public GameObject unique;
    public GameObject epic;
    public GameObject legendary;

    public CompanionDataSO companionData;
    public SkillDataSO skillData;
    public EquipmentDataSO weaponData;

    private void Start()
    {
        OpenButton.onClick.AddListener(OpenScroll);
    }

    public void Setup(CompanionDataSO companion)
    {
        companionData = companion;
        SetupCommon();
    }

    public void Setup(SkillDataSO skill)
    {
        skillData = skill;
        SetupCommon();
    }

    public void Setup(EquipmentDataSO weapon)
    {
        weaponData = weapon;
        SetupCommon();
    }

    private void SetupCommon()
    {
        front.SetActive(false);
        back.SetActive(true);

        normal.SetActive(false);
        rare.SetActive(false);
        unique.SetActive(false);
        epic.SetActive(false);
        legendary.SetActive(false);
    }

    public void OpenScroll()
    {
        back.SetActive(false);
        front.SetActive(true);

        if (companionData != null)
        {
            icon.sprite = companionData.Icon;
            switch (companionData.Rarity)
            {
                case Define.SkillRarity.Normal:
                    normal.SetActive(true);
                    break;
                case Define.SkillRarity.Rare:
                    rare.SetActive(true);
                    break;
                case Define.SkillRarity.Unique:
                    unique.SetActive(true);
                    break;
                case Define.SkillRarity.Epic:
                    epic.SetActive(true);
                    break;
                case Define.SkillRarity.Legendary:
                    legendary.SetActive(true);
                    break;
            }
        }
        else if (skillData != null)
        {
            icon.sprite = skillData.Icon;
            switch (skillData.Rarity)
            {
                case Define.SkillRarity.Normal:
                    normal.SetActive(true);
                    break;
                case Define.SkillRarity.Rare:
                    rare.SetActive(true);
                    break;
                case Define.SkillRarity.Unique:
                    unique.SetActive(true);
                    break;
                case Define.SkillRarity.Epic:
                    epic.SetActive(true);
                    break;
                case Define.SkillRarity.Legendary:
                    legendary.SetActive(true);
                    break;
            }
        }
        else if (weaponData != null)
        {
            icon.sprite = weaponData.Sprite;
            switch (weaponData.GachaRarity)
            {
                case (Define.GachaRarity)Define.SkillRarity.Normal:
                    normal.SetActive(true);
                    break;
                case (Define.GachaRarity)Define.SkillRarity.Rare:
                    rare.SetActive(true);
                    break;
                case (Define.GachaRarity)Define.SkillRarity.Unique:
                    unique.SetActive(true);
                    break;
                case (Define.GachaRarity)Define.SkillRarity.Epic:
                    epic.SetActive(true);
                    break;
                case (Define.GachaRarity)Define.SkillRarity.Legendary:
                    legendary.SetActive(true);
                    break;
            }
        }
    }
}

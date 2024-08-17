using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public GameObject gachaPage;
    public GameObject scrollGrid;
    public GameObject scrollPrefab;

    [Header("Data")]
    public List<CompanionDataSO> companionDataList;
    public List<SkillDataSO> skillDataList;
    public List<EquipmentDataSO> weaponDataList;

    [Header("Button")]
    public Button oneCompanionPullButton;
    public Button twelveCompanionPullButton;
    public Button oneSkillPullButton;
    public Button twelveSkillPullButton;
    public Button oneWeaponPullButton;
    public Button twelveWeaponPullButton;
    public Button closeButton;
    public Button allOpenButton;

    private SkillUIManager skillUIManager;
    private List<GachaScroll> activeScrolls = new List<GachaScroll>();

    private void Start()
    {
        closeButton.onClick.AddListener(CloseGacha);
        allOpenButton.onClick.AddListener(AllScrollOpen);

        oneCompanionPullButton.onClick.AddListener(() => PullGacha(1, "Companion"));
        twelveCompanionPullButton.onClick.AddListener(() => PullGacha(12, "Companion"));
        oneSkillPullButton.onClick.AddListener(() => PullGacha(1, "Skill"));
        twelveSkillPullButton.onClick.AddListener(() => PullGacha(12, "Skill"));
        oneWeaponPullButton.onClick.AddListener(() => PullGacha(1, "Weapon"));
        twelveWeaponPullButton.onClick.AddListener(() => PullGacha(12, "Weapon"));
        skillUIManager = FindObjectOfType<SkillUIManager>();
    }

    public void PullGacha(int pullCount, string type)
    {
        if (pullCount == 1 && DataManager.Instance.PlayerDataSo.Gem >= 100)
        {
            DataManager.Instance.PlayerDataSo.Gem -= 100;
        }
        else if (pullCount == 12 && DataManager.Instance.PlayerDataSo.Gem >= 1000)
        {
            DataManager.Instance.PlayerDataSo.Gem -= 1000;
        }
        else
        {
            return;
        }

        gachaPage.SetActive(true);
        closeButton.SetActive(true);
        allOpenButton.SetActive(true);

        for (int i = 0; i < pullCount; i++)
        {
            GameObject scrollInstance = Instantiate(scrollPrefab, scrollGrid.transform);
            GachaScroll scroll = scrollInstance.GetComponent<GachaScroll>();
            if (type == "Companion")
            {
                CompanionDataSO pulledCompanion = GetRandomCompanion();
                scroll.Setup(pulledCompanion);
            }
            else if (type == "Skill")
            {
                SkillDataSO pulledSkill = GetRandomSkill();
                scroll.Setup(pulledSkill);
            }
            else if (type == "Weapon")
            {
                EquipmentDataSO pulledWeapon = GetRandomWeapon();
                scroll.Setup(pulledWeapon);
            }
            activeScrolls.Add(scroll);
        }
    }

    public CompanionDataSO GetRandomCompanion()
    {
        int totalWeight = 0;
        foreach (CompanionDataSO companion in companionDataList)
        {
            totalWeight += GetWeightByRarity(companion.Rarity);
        }

        int randomValue = Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        foreach (CompanionDataSO companion in companionDataList)
        {
            accumulatedWeight += GetWeightByRarity(companion.Rarity);
            if (randomValue < accumulatedWeight)
            {
                companion.Count++;
                return companion;
            }
        }

        return null;
    }

    public SkillDataSO GetRandomSkill()
    {
        int totalWeight = 0;
        foreach (SkillDataSO skill in skillDataList)
        {
            totalWeight += GetWeightByRarity(skill.Rarity);
        }

        int randomValue = Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        foreach (SkillDataSO skill in skillDataList)
        {
            accumulatedWeight += GetWeightByRarity(skill.Rarity);
            if (randomValue < accumulatedWeight)
            {
                skill.Count++;
                skillUIManager.AcquireSkill(skill);
                return skill;
            }
        }

        return null;
    }

    public EquipmentDataSO GetRandomWeapon()
    {
        int totalWeight = 0;
        foreach (EquipmentDataSO weapon in weaponDataList)
        {
            totalWeight += GetWeightByRarity((Define.SkillRarity)weapon.GachaRarity);
        }

        int randomValue = Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        foreach (EquipmentDataSO weapon in weaponDataList)
        {
            accumulatedWeight += GetWeightByRarity((Define.SkillRarity)weapon.GachaRarity);
            if (randomValue < accumulatedWeight)
            {
                weapon.Count++;
                return weapon;
            }
        }

        return null;
    }

    private int GetWeightByRarity(Define.SkillRarity rarity)
    {
        switch (rarity)
        {
            case Define.SkillRarity.Normal:
                return 70;
            case Define.SkillRarity.Rare:
                return 15;
            case Define.SkillRarity.Unique:
                return 9;
            case Define.SkillRarity.Epic:
                return 5;
            case Define.SkillRarity.Legendary:
                return 1;
            default:
                return 0;
        }
    }

    private void CloseGacha()
    {
        foreach (Transform child in scrollGrid.transform)
        {
            Destroy(child.gameObject);
        }
        activeScrolls.Clear();

        gachaPage.SetActive(false);
        closeButton.SetActive(false);
        allOpenButton.SetActive(false);
    }

    private void AllScrollOpen()
    {
        foreach (var scroll in activeScrolls)
        {
            scroll.OpenScroll();
        }
    }
}

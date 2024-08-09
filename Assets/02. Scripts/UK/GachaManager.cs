using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public GameObject gachaPage;
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
        // ����Ƽ ��ư ������Ʈ���� �������� �ʰ� �ڵ�� �����غ���
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
        // ��í������ Ȱ��ȭ
        gachaPage.SetActive(true);
        closeButton.SetActive(true);
        allOpenButton.SetActive(true);

        for (int i = 0; i < pullCount; i++)
        {
            GameObject scrollInstance = Instantiate(scrollPrefab, gachaPage.transform);
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
        // ���⿡ ��޿� ���� Ȯ�� ��� ����
        // Rarity�� ���� ���� ���� Ȯ���� ������
        int totalWeight = 0;
        foreach (CompanionDataSO companion in companionDataList)
        {
            totalWeight += GetWeightByRarity(companion.rarity);
        }

        int randomValue = Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        foreach (CompanionDataSO companion in companionDataList)
        {
            accumulatedWeight += GetWeightByRarity(companion.rarity);
            if (randomValue < accumulatedWeight)
            {
                companion.count++;
                return companion;
            }
        }

        return null; // ������ġ, �� �ڵ忡 �����ϸ� �� ��.
    }

    public SkillDataSO GetRandomSkill()
    {
        int totalWeight = 0;
        foreach (SkillDataSO skill in skillDataList)
        {
            totalWeight += GetWeightByRarity(skill.rarity);
        }

        int randomValue = Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        foreach (SkillDataSO skill in skillDataList)
        {
            accumulatedWeight += GetWeightByRarity(skill.rarity);
            if (randomValue < accumulatedWeight)
            {
                skill.count++;
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
            totalWeight += GetWeightByRarity((Define.SkillRarity)weapon.gachaRarity);
        }

        int randomValue = Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        foreach (EquipmentDataSO weapon in weaponDataList)
        {
            accumulatedWeight += GetWeightByRarity((Define.SkillRarity)weapon.gachaRarity);
            if (randomValue < accumulatedWeight)
            {
                weapon.count++;
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
        // ��� Scroll ������Ʈ ����
        foreach (Transform child in gachaPage.transform)
        {
            Destroy(child.gameObject);
        }
        // ����Ʈ �ʱ�ȭ
        activeScrolls.Clear();

        // GachaPage ��Ȱ��ȭ
        gachaPage.SetActive(false);
        closeButton.SetActive(false);
        allOpenButton.SetActive(false);
    }

    private void AllScrollOpen()
    {
        // Ȱ��ȭ�� ��� Scroll�� Front�� Ȱ��ȭ
        foreach (var scroll in activeScrolls)
        {
            scroll.OpenScroll();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private MainSceneSkillManager mainSceneSkillManager;
    [SerializeField] private GameObject skillItemPrefab;
    [SerializeField] private Transform allSkillsContainer;
    [SerializeField] private GameObject skillInfoPanel;
    [SerializeField] private Text instructionText;
    [SerializeField] private List<Button> equippedSkillSlots;

    private SkillInfoPanel skillInfoPanelScript;

    private void Start()
    {
        skillManager.OnEquippedSkillsChanged += RefreshSkillUI;
        skillInfoPanelScript = skillInfoPanel.GetComponent<SkillInfoPanel>();
        RefreshSkillUI();
    }

    private void OnDestroy()
    {
        skillManager.OnEquippedSkillsChanged -= RefreshSkillUI;
    }

    public void RefreshSkillUI()
    {
        UpdateEquippedSkillSlots();
        UpdateAllSkillsContainer();
        mainSceneSkillManager.UpdateSkillButtons();
    }

    private void UpdateEquippedSkillSlots()
    {
        for (int i = 0; i < equippedSkillSlots.Count; i++)
        {
            Image iconImage = equippedSkillSlots[i].GetComponent<Image>();
            Text levelText = equippedSkillSlots[i].GetComponentInChildren<Text>();

            if (i < skillManager.equippedSkills.Count)
            {
                SkillDataSO skill = skillManager.equippedSkills[i];
                iconImage.sprite = skill.icon;
                levelText.text = $"Lv.{skill.level}";
            }
            else
            {
                levelText.text = "";
            }
        }
    }

    private void UpdateAllSkillsContainer()
    {
        foreach (Transform child in allSkillsContainer)
        {
            Destroy(child.gameObject);
        }

        List<SkillDataSO> sortedSkills = new List<SkillDataSO>(DataManager.Instance.allSkillsDataSO);
        sortedSkills.Sort(CompareSkillsByRarity);

        foreach (var skill in sortedSkills)
        {
            CreateSkillItem(skill, allSkillsContainer);
        }
    }

    private int CompareSkillsByRarity(SkillDataSO a, SkillDataSO b)
    {
        return a.rarity.CompareTo(b.rarity);
    }

    private void CreateSkillItem(SkillDataSO skill, Transform container)
    {
        GameObject skillItemObj = Instantiate(skillItemPrefab, container);
        Button button = skillItemObj.GetComponent<Button>();
        Image iconImage = button.GetComponent<Image>();
        Text levelText = button.GetComponentInChildren<Text>();

        iconImage.sprite = skill.icon;
        levelText.text = $"Lv.{skill.level}";

        button.onClick.AddListener(() => ShowSkillInfo(skill));
    }

    private void ShowSkillInfo(SkillDataSO skill)
    {
        skillInfoPanel.SetActive(true);
        skillInfoPanelScript.SetSkillInfo(skill);

        skillInfoPanelScript.equipButton.onClick.RemoveAllListeners();
        skillInfoPanelScript.equipButton.onClick.AddListener(() => StartEquipProcess(skill));
    }

    public void StartEquipProcess(SkillDataSO skill)
    {
        skillInfoPanel.SetActive(false);
        instructionText.text = "��ü�� ��ų ������ �����ϼ���";
        SetEquippedSkillsInteractable(true, skill);
    }

    private void SetEquippedSkillsInteractable(bool interactable, SkillDataSO skillToEquip = null)
    {
        foreach (var button in equippedSkillSlots)
        {
            button.interactable = interactable;
            if (interactable && skillToEquip != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => EquipSkillToSlot(button, skillToEquip));
            }
            else
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }

    private void EquipSkillToSlot(Button slot, SkillDataSO newSkill)
    {
        int index = equippedSkillSlots.IndexOf(slot);
        if (index != -1)
        {
            if (index < skillManager.equippedSkills.Count)
            {
                skillManager.ReplaceSkill(index, newSkill);
            }
            else
            {
                skillManager.EquipSkill(newSkill);
            }
        }

        SetEquippedSkillsInteractable(false);
        instructionText.text = "";
    }

    public void UnequipSkill(SkillDataSO skill)
    {
        skillManager.UnequipSkill(skill);
    }
    public static string GetRarityString(Define.SkillRarity rarity)
    {
        switch (rarity)
        {
            case Define.SkillRarity.Normal: return "�븻";
            case Define.SkillRarity.Rare: return "����";
            case Define.SkillRarity.Unique: return "����ũ";
            case Define.SkillRarity.Epic: return "����";
            case Define.SkillRarity.Legendary: return "��������";
            default: return "�� �� ����";
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    public SkillManager skillManager;
    public MainSceneSkillManager mainSceneSkillManager;
    public GameObject skillItemPrefab;
    public Transform equippedSkillsContainer;
    public Transform allSkillsContainer;
    public GameObject skillInfoPanel;
    private SkillInfoPanel skillInfoPanelScript;
    public Text instructionText;
    public List<Button> equippedSkillSlots;


    private void Start()
    {
        RefreshSkillUI();
        skillManager.OnEquippedSkillsChanged += OnEquippedSkillsChanged;
        skillInfoPanelScript = skillInfoPanel.GetComponent<SkillInfoPanel>();
    }

    private void OnDestroy()
    {
        skillManager.OnEquippedSkillsChanged -= OnEquippedSkillsChanged;
    }

    private void OnEquippedSkillsChanged()
    {
        RefreshSkillUI();
        mainSceneSkillManager.UpdateSkillButtons();
    }

    public void RefreshSkillUI()
    {
        for (int i = 0; i < equippedSkillSlots.Count; i++)
        {
            Image iconImage = equippedSkillSlots[i].GetComponent<Image>();
            Text levelText = equippedSkillSlots[i].GetComponentInChildren<Text>();

            if (i < skillManager.equippedSkills.Count)
            {
                SkillDataSO skill = skillManager.equippedSkills[i];
                iconImage.sprite = skill.icon;
                iconImage.color = Color.white;
                levelText.text = $"Lv.{skill.level}";
            }
            else
            {
                levelText.text = "";
            }
        }

        // Update all skills
        ClearContainer(allSkillsContainer);
        foreach (var skill in skillManager.allSkills)
        {
            CreateSkillItem(skill, allSkillsContainer, false);
        }
    }

    private void CreateSkillItem(SkillDataSO skill, Transform container, bool isEquipped)
    {
        GameObject skillItemObj = Instantiate(skillItemPrefab, container);
        Button button = skillItemObj.GetComponent<Button>();
        Image iconImage = button.GetComponent<Image>();
        Text levelText = button.GetComponentInChildren<Text>();

        iconImage.sprite = skill.icon;
        levelText.text = $"Lv.{skill.level}";

        button.onClick.AddListener(() => ShowSkillInfo(skill, isEquipped));
    }

    private void ClearContainer(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowSkillInfo(SkillDataSO skill, bool isEquipped)
    {
        skillInfoPanel.SetActive(true);
        skillInfoPanelScript.SetSkillInfo(skill);

        skillInfoPanelScript.equipButton.onClick.RemoveAllListeners();
        skillInfoPanelScript.equipButton.onClick.AddListener(() => StartEquipProcess(skill));
        // 스킬 정보를 패널에 표시
        // 장착/해제 버튼 설정
        // 강화 버튼 설정 (필요시)
    }

    public void StartEquipProcess(SkillDataSO skill)
    {
        skillInfoPanel.SetActive(false);
        instructionText.text = "교체할 스킬 슬롯을 선택하세요";
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

        RefreshSkillUI();
        SetEquippedSkillsInteractable(false);
        instructionText.text = "";
    }

    public void UnequipSkill(SkillDataSO skill)
    {
        skillManager.UnequipSkill(skill);
        RefreshSkillUI();
    }
}

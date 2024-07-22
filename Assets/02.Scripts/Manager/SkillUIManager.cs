using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    public SkillManager skillManager;
    public MainSceneSkillManager mainSceneSkillManager;
    public GameObject skillItemPrefab;
    public Transform equippedSkillsContainer;
    public Transform allSkillsContainer;
    public GameObject skillInfoPanel;

    private SkillInfoPanel skillInfoPanelScript;


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
        ClearContainer(equippedSkillsContainer);
        ClearContainer(allSkillsContainer);

        foreach (var skill in skillManager.equippedSkills)
        {
            CreateSkillItem(skill, equippedSkillsContainer, true);
        }

        foreach (var skill in skillManager.allSkills)
        {
            CreateSkillItem(skill, allSkillsContainer, false);
        }
    }

    private void CreateSkillItem(SkillDataSO skill, Transform container, bool isEquipped)
    {
        GameObject skillItemObj = Instantiate(skillItemPrefab, container);
        SkillUIItem uiItem = skillItemObj.GetComponent<SkillUIItem>();
        uiItem.SetSkill(skill, isEquipped);
        uiItem.OnClick += () => ShowSkillInfo(skill, isEquipped);
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
        skillInfoPanelScript.equipButton.onClick.AddListener(skillInfoPanelScript.OnClickEquipButton);
        // 스킬 정보를 패널에 표시
        // 장착/해제 버튼 설정
        // 강화 버튼 설정 (필요시)
    }

    public void EquipSkill(SkillDataSO skill)
    {
        skillManager.EquipSkill(skill);
        RefreshSkillUI();
    }

    public void UnequipSkill(SkillDataSO skill)
    {
        skillManager.UnequipSkill(skill);
        RefreshSkillUI();
    }
}
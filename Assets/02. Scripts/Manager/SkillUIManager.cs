using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private MainSceneSkillManager mainSceneSkillManager;
    [SerializeField] private GameObject skillItemPrefab;
    [SerializeField] private Transform allSkillsContainer;
    [SerializeField] private GameObject skillInfoPanel;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private List<Button> equippedSkillSlots;
    [SerializeField] private Button batchEnhanceButton;

    private SkillInfoPanel skillInfoPanelScript;

    private void Start()
    {
        skillManager.OnEquippedSkillsChanged += RefreshSkillUI;
        skillInfoPanelScript = skillInfoPanel.GetComponent<SkillInfoPanel>();
        RefreshSkillUI();
        batchEnhanceButton.onClick.AddListener(OnBatchEnhanceButtonClick);
        UpdateBatchEnhanceButtonState();
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
        UpdateBatchEnhanceButtonState();
    }

    private void UpdateBatchEnhanceButtonState()
    {
        bool canEnhanceAny = DataManager.Instance.AllSkillsDataSo.Any(skill => skill.IsUnlocked && skill.Count >= 5);
        batchEnhanceButton.interactable = canEnhanceAny;
    }

    public void OnBatchEnhanceButtonClick()
    {
        bool enhanced = false;
        foreach (var skill in DataManager.Instance.AllSkillsDataSo)
        {
            if (skill.IsUnlocked && skill.Count >= 5)
            {
                EnhanceSkill(skill);
                enhanced = true;
            }
        }

        if (enhanced)
        {
            RefreshSkillUI();
            Debug.Log("일괄 강화가 완료되었습니다.");
        }
        else
        {
            Debug.Log("강화할 수 있는 스킬이 없습니다.");
        }
    }

    private void EnhanceSkill(SkillDataSO skill)
    {
        skill.Level++;
        skill.Count -= 5;
        switch (skill.SkillType)
        {
            case Define.SkillType.AttackBuff:
                skill.BuffAmount += 5;
                break;
            case Define.SkillType.HealBuff:
                skill.BuffAmount += 10;
                break;
            default:
                skill.Damage += 10;
                break;
        }
        Debug.Log($"{skill.SkillName} 강화 완료! 현재 레벨: {skill.Level}");
    }

    private void UpdateEquippedSkillSlots()
    {
        for (int i = 0; i < equippedSkillSlots.Count; i++)
        {
            Image iconImage = equippedSkillSlots[i].GetComponent<Image>();
            TextMeshProUGUI levelText = equippedSkillSlots[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i < skillManager.equippedSkills.Count && skillManager.equippedSkills[i] != null)
            {
                SkillDataSO skill = skillManager.equippedSkills[i];
                iconImage.sprite = skill.Icon;
                iconImage.color = Color.white;
                levelText.text = $"Lv.{skill.Level}";
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

        List<SkillDataSO> sortedSkills = new List<SkillDataSO>(DataManager.Instance.AllSkillsDataSo);
        sortedSkills.Sort(CompareSkillsByRarity);

        foreach (var skill in sortedSkills)
        {
            CreateSkillItem(skill, allSkillsContainer, skill.IsUnlocked);
        }
    }

    private int CompareSkillsByRarity(SkillDataSO a, SkillDataSO b)
    {
        return a.Rarity.CompareTo(b.Rarity);
    }

    private void CreateSkillItem(SkillDataSO skill, Transform container, bool isUnlocked)
    {
        GameObject skillItemObj = Instantiate(skillItemPrefab, container);
        Button button = skillItemObj.GetComponent<Button>();
        Image iconImage = button.GetComponent<Image>();
        TextMeshProUGUI levelText = button.GetComponentInChildren<TextMeshProUGUI>();

        iconImage.sprite = skill.Icon;
        levelText.text = $"Lv.{skill.Level}";

        button.interactable = isUnlocked;
        button.onClick.AddListener(() => ShowSkillInfo(skill));
    }

    public void AcquireSkill(SkillDataSO skill)
    {
        skill.IsUnlocked = true;
        RefreshSkillUI();
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
        instructionText.text = "교체할 스킬 슬롯을 선택하세요.";
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
        // ���� ������ ��ų�� �˻�
        int index = equippedSkillSlots.IndexOf(slot);
        if (index == -1) return;

        // �̹� ���� ��ų�� �����Ǿ� �ִ��� Ȯ��
        bool isSkillAlreadyEquipped = skillManager.equippedSkills.Exists(skill => skill != null && skill == newSkill);
        if (isSkillAlreadyEquipped)
        {
            Debug.LogWarning("�̹� �� ��ų�� �����Ǿ� �ֽ��ϴ�.");
            instructionText.text = "이미 장착된 스킬입니다.";
            return;
        }

        // ��ų�� ����
        skillManager.EquipSkillAtIndex(index, newSkill);

        // UI ������Ʈ
        SetEquippedSkillsInteractable(false);
        instructionText.text = "";
        RefreshSkillUI();
    }

    public void UnequipSkill(SkillDataSO skill)
    {
        skillManager.UnequipSkill(skill);
        RefreshSkillUI();
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
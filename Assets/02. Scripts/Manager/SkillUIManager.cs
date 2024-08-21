using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    [FormerlySerializedAs("skillManager")]
    [SerializeField]
    private SkillManager _skillManager;

    [FormerlySerializedAs("mainSceneSkillManager")]
    [SerializeField]
    private MainSceneSkillManager _mainSceneSkillManager;

    [FormerlySerializedAs("skillItemPrefab")]
    [SerializeField]
    private GameObject _skillItemPrefab;

    [FormerlySerializedAs("allSkillsContainer")]
    [SerializeField]
    private Transform _allSkillsContainer;

    [FormerlySerializedAs("skillInfoPanel")]
    [SerializeField]
    private GameObject _skillInfoPanel;

    [FormerlySerializedAs("instructionText")]
    [SerializeField]
    private TextMeshProUGUI _instructionText;

    [FormerlySerializedAs("equippedSkillSlots")]
    [SerializeField]
    private List<Button> _equippedSkillSlots;

    [FormerlySerializedAs("batchEnhanceButton")]
    [SerializeField]
    private Button _batchEnhanceButton;

    [SerializeField]
    private Sprite _defaultSlotSprite;


    [SerializeField]
    private SkillInfoPanel _skillInfoPanelScript;

    private void Start()
    {
        _skillManager.onEquippedSkillsChanged += RefreshSkillUI;
        _skillInfoPanelScript = _skillInfoPanel.GetComponent<SkillInfoPanel>();
        RefreshSkillUI();
        _batchEnhanceButton.onClick.AddListener(OnBatchEnhanceButtonClick);
    }
    private void OnDestroy()
    {
        _skillManager.onEquippedSkillsChanged -= RefreshSkillUI;
    }

    public void RefreshSkillUI()
    {
        UpdateEquippedSkillSlots();
        UpdateAllSkillsContainer();
        _mainSceneSkillManager.UpdateSkillButtons();
        UpdateBatchEnhanceButtonState();
    }

    private void UpdateBatchEnhanceButtonState()
    {
        var canEnhanceAny = DataManager.Instance.AllSkillsDataSo.Any(skill => skill.IsUnlocked && skill.Count >= 5);
        _batchEnhanceButton.interactable = canEnhanceAny;
    }

    private void OnBatchEnhanceButtonClick()
    {
        var enhanced = false;
        foreach (var skill in DataManager.Instance.AllSkillsDataSo.Where(skill => skill.IsUnlocked && skill.Count >= 5))
        {
            EnhanceSkill(skill);
            enhanced = true;
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

    private static void EnhanceSkill(SkillDataSO skill)
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
            case Define.SkillType.Projectile:
            case Define.SkillType.AreaOfEffect:
            default:
                skill.Damage += 10;
                break;
        }

        Debug.Log($"{skill.SkillName} 강화 완료! 현재 레벨: {skill.Level}");
    }

    private void UpdateEquippedSkillSlots()
    {
        for (var i = 0; i < _equippedSkillSlots.Count; i++)
        {
            var button = _equippedSkillSlots[i];
            var iconImage = button.GetComponent<Image>();
            var levelText = button.GetComponentInChildren<TextMeshProUGUI>();

            button.onClick.RemoveAllListeners();

            if (i < DataManager.Instance.PlayerDataSo.EquippedSkills.Count && DataManager.Instance.PlayerDataSo.EquippedSkills[i] != null)
            {
                SkillDataSO skill = DataManager.Instance.PlayerDataSo.EquippedSkills[i];
                iconImage.sprite = skill.Icon;
                iconImage.color = Color.white;
                levelText.text = $"Lv.{skill.Level}";

                int index = i;
                button.onClick.AddListener(() => ShowEquippedSkillInfo(skill, index));
                button.interactable = true;
            }
            else
            {
                iconImage.sprite = _defaultSlotSprite;
                levelText.text = "";
                button.interactable = false;
            }
        }
    }

    private void ShowEquippedSkillInfo(SkillDataSO skill, int slotIndex)
    {
        _skillInfoPanel.SetActive(true);
        _skillInfoPanelScript.SetSkillInfo(skill, slotIndex);
    }

    public void UnequipSkillAtIndex(int index)
    {
        if (index >= 0 && index < DataManager.Instance.PlayerDataSo.EquippedSkills.Count)
        {
            DataManager.Instance.PlayerDataSo.EquippedSkills[index] = null;
            RefreshSkillUI();
            UpdateEquippedSkillSlots();
            _mainSceneSkillManager.UpdateSkillButtons();
        }
    }

    private void UpdateAllSkillsContainer()
    {
        foreach (Transform child in _allSkillsContainer)
        {
            Destroy(child.gameObject);
        }

        var sortedSkills = new List<SkillDataSO>(DataManager.Instance.AllSkillsDataSo);
        sortedSkills.Sort(CompareSkillsByRarity);

        foreach (var skill in sortedSkills)
        {
            CreateSkillItem(skill, _allSkillsContainer, skill.IsUnlocked);
        }
    }

    private static int CompareSkillsByRarity(SkillDataSO a, SkillDataSO b)
    {
        return a.Rarity.CompareTo(b.Rarity);
    }

    private void CreateSkillItem(SkillDataSO skill, Transform container, bool isUnlocked)
    {
        var skillItemObj = Instantiate(_skillItemPrefab, container);
        var button = skillItemObj.GetComponent<Button>();
        var iconImage = button.GetComponent<Image>();
        var levelText = button.GetComponentInChildren<TextMeshProUGUI>();

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
        _skillInfoPanel.SetActive(true);
        _skillInfoPanelScript.SetSkillInfo(skill, -1);

        _skillInfoPanelScript.equipButton.onClick.RemoveAllListeners();
        _skillInfoPanelScript.equipButton.onClick.AddListener(() => StartEquipProcess(skill));
    }

    public void StartEquipProcess(SkillDataSO skill)
    {
        _skillInfoPanel.SetActive(false);
        _instructionText.text = "교체할 스킬 슬롯을 선택하세요.";
        SetEquippedSkillsInteractable(true, skill);
    }

    private void SetEquippedSkillsInteractable(bool interactable, SkillDataSO skillToEquip = null)
    {
        foreach (var button in _equippedSkillSlots)
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
        var index = _equippedSkillSlots.IndexOf(slot);
        if (index == -1) return;

        // �̹� ���� ��ų�� �����Ǿ� �ִ��� Ȯ��
        var isSkillAlreadyEquipped = DataManager.Instance.PlayerDataSo.EquippedSkills.Exists(skill => skill != null && skill == newSkill);
        if (isSkillAlreadyEquipped)
        {
            Debug.LogWarning("�̹� �� ��ų�� �����Ǿ� �ֽ��ϴ�.");
            _instructionText.text = "이미 장착된 스킬입니다.";
            StartCoroutine(ClearInstructionTextAfterDelay(1f));
            return;
        }

        // ��ų�� ����
        _skillManager.EquipSkillAtIndex(index, newSkill);

        // UI ������Ʈ
        SetEquippedSkillsInteractable(false);
        _instructionText.text = "";
        RefreshSkillUI();
    }

    private IEnumerator ClearInstructionTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _instructionText.text = "";
    }

    public void UnequipSkill(SkillDataSO skill)
    {
        _skillManager.UnEquipSkill(skill);
        RefreshSkillUI();
    }

    public static string GetRarityString(Define.SkillRarity rarity)
    {
        return rarity switch
        {
            Define.SkillRarity.Normal => "�븻",
            Define.SkillRarity.Rare => "����",
            Define.SkillRarity.Unique => "����ũ",
            Define.SkillRarity.Epic => "����",
            Define.SkillRarity.Legendary => "��������",
            _ => "�� �� ����"
        };
    }

    public void DebugPrintEquippedSkills()
    {
        for (int i = 0; i < DataManager.Instance.PlayerDataSo.EquippedSkills.Count; i++)
        {
            var skill = DataManager.Instance.PlayerDataSo.EquippedSkills[i];
            Debug.Log($"Slot {i}: {(skill != null ? skill.SkillName : "Empty")}");
        }
    }
}
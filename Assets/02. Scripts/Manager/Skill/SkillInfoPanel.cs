using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInfoPanel : MonoBehaviour
{
    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillLevel;
    public TextMeshProUGUI skillEffect;
    public TextMeshProUGUI skillCount;

    public Button equipButton;
    public Button enhanceButton;
    private SkillDataSO currentSkill;
    private SkillUIManager skillUIManager;

    public Button unequipButton;
    private int currentSkillSlotIndex = -1;

    private void Start()
    {
        skillUIManager = FindObjectOfType<SkillUIManager>();
        unequipButton.onClick.AddListener(OnClickUnequipButton);
    }

    public void OnClickUnequipButton()
    {
        if (currentSkill != null && currentSkillSlotIndex != -1)
        {
            skillUIManager.UnequipSkillAtIndex(currentSkillSlotIndex);
            gameObject.SetActive(false);
        }
    }

    public void SetSkillInfo(SkillDataSO skill, int slotIndex)
    {
        currentSkill = skill;
        currentSkillSlotIndex = slotIndex;
        UpdateSkillInfo();

        unequipButton.gameObject.SetActive(slotIndex != -1);
        equipButton.gameObject.SetActive(slotIndex == -1);
    }

    private void UpdateSkillInfo()
    {
        skillIcon.sprite = currentSkill.Icon;
        skillName.text = currentSkill.SkillName;
        skillDescription.text = currentSkill.Description;
        skillLevel.text = "Level: " + currentSkill.Level.ToString();
        skillCount.text = currentSkill.Count.ToString() + " / 5";
        switch (currentSkill.SkillType)
        {
            case Define.SkillType.AttackBuff:
                skillEffect.text = "Attack Buff: +" + currentSkill.BuffAmount.ToString();
                break;
            case Define.SkillType.HealBuff:
                skillEffect.text = "Heal Amount: " + currentSkill.BuffAmount.ToString();
                break;
            default:
                skillEffect.text = "Damage: " + currentSkill.Damage.ToString();
                break;
        }

        enhanceButton.interactable = (currentSkill.Count >= 5);
    }

    public void OnClickEquipButton()
    {
        if(currentSkill != null)
        {
            SkillUIManager skillUIManager = FindObjectOfType<SkillUIManager>();
            skillUIManager.StartEquipProcess(currentSkill);

            gameObject.SetActive(false);
        }
    }
    public void OnClickEnhanceButton()
    {
        if (currentSkill != null)
        {
            // ���⿡ ��ȭ�� �ʿ��� ���� (��: ���, ������ ��) Ȯ�� ���� �߰�
            currentSkill.Level++;
            currentSkill.Count -= 5;
            switch (currentSkill.SkillType)
            {
                case Define.SkillType.AttackBuff:
                    currentSkill.BuffAmount += 5; // ���ݷ� ���� ������, �뷱���� �°� ���� �ʿ�
                    Debug.Log($"{currentSkill.SkillName} ��ȭ ����! ���� ����: {currentSkill.Level}, Attack Buff: +{currentSkill.BuffAmount}");
                    break;
                case Define.SkillType.HealBuff:
                    currentSkill.BuffAmount += 10; // ġ���� ������, �뷱���� �°� ���� �ʿ�
                    Debug.Log($"{currentSkill.SkillName} ��ȭ ����! ���� ����: {currentSkill.Level}, Heal Amount: {currentSkill.BuffAmount}");
                    break;
                default:
                    currentSkill.Damage += 10; // ������ �������� ���� �뷱���� �°� ����
                    Debug.Log($"{currentSkill.SkillName} ��ȭ ����! ���� ����: {currentSkill.Level}, Damage: {currentSkill.Damage}");
                    break;
            }

            UpdateSkillInfo();
            // SkillUIManager�� RefreshSkillUI �޼��� ȣ��
            if (skillUIManager != null)
            {
                skillUIManager.RefreshSkillUI();
            }
        }
        else
        {
            Debug.Log("��ȭ�� �ʿ��� ��ų ī��Ʈ�� �����մϴ�.");
        }
    }
}
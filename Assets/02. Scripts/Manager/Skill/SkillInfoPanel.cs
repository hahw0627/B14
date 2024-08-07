using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class SkillInfoPanel : MonoBehaviour
{
    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillLevel;
    public TextMeshProUGUI skillEffect;

    public Button equipButton;
    public Button enhanceButton;
    private SkillDataSO currentSkill;
    private SkillUIManager skillUIManager;

    private void Start()
    {
        skillUIManager = FindObjectOfType<SkillUIManager>();
    }

    public void SetSkillInfo(SkillDataSO skill)
    {
        currentSkill = skill;
        UpdateSkillInfo();
    }

    private void UpdateSkillInfo()
    {
        skillIcon.sprite = currentSkill.icon;
        skillName.text = currentSkill.skillName;
        skillDescription.text = currentSkill.description;
        skillLevel.text = "Level: " + currentSkill.level.ToString();
        switch (currentSkill.skillType)
        {
            case Define.SkillType.AttackBuff:
                skillEffect.text = "Attack Buff: +" + currentSkill.buffAmount.ToString();
                break;
            case Define.SkillType.HealBuff:
                skillEffect.text = "Heal Amount: " + currentSkill.buffAmount.ToString();
                break;
            default:
                skillEffect.text = "Damage: " + currentSkill.damage.ToString();
                break;
        }
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

            currentSkill.level++;
            switch (currentSkill.skillType)
            {
                case Define.SkillType.AttackBuff:
                    currentSkill.buffAmount += 5; // ���ݷ� ���� ������, �뷱���� �°� ���� �ʿ�
                    Debug.Log($"{currentSkill.skillName} ��ȭ ����! ���� ����: {currentSkill.level}, Attack Buff: +{currentSkill.buffAmount}");
                    break;
                case Define.SkillType.HealBuff:
                    currentSkill.buffAmount += 10; // ġ���� ������, �뷱���� �°� ���� �ʿ�
                    Debug.Log($"{currentSkill.skillName} ��ȭ ����! ���� ����: {currentSkill.level}, Heal Amount: {currentSkill.buffAmount}");
                    break;
                default:
                    currentSkill.damage += 10; // ������ �������� ���� �뷱���� �°� ����
                    Debug.Log($"{currentSkill.skillName} ��ȭ ����! ���� ����: {currentSkill.level}, Damage: {currentSkill.damage}");
                    break;
            }

            UpdateSkillInfo();
            // SkillUIManager�� RefreshSkillUI �޼��� ȣ��
            if (skillUIManager != null)
            {
                skillUIManager.RefreshSkillUI();
            }

        }
    }
}

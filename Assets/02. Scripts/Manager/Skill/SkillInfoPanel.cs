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
            // 여기에 강화에 필요한 조건 (예: 골드, 아이템 등) 확인 로직 추가

            currentSkill.level++;
            switch (currentSkill.skillType)
            {
                case Define.SkillType.AttackBuff:
                    currentSkill.buffAmount += 5; // 공격력 버프 증가량, 밸런스에 맞게 조정 필요
                    Debug.Log($"{currentSkill.skillName} 강화 성공! 현재 레벨: {currentSkill.level}, Attack Buff: +{currentSkill.buffAmount}");
                    break;
                case Define.SkillType.HealBuff:
                    currentSkill.buffAmount += 10; // 치유량 증가량, 밸런스에 맞게 조정 필요
                    Debug.Log($"{currentSkill.skillName} 강화 성공! 현재 레벨: {currentSkill.level}, Heal Amount: {currentSkill.buffAmount}");
                    break;
                default:
                    currentSkill.damage += 10; // 데미지 증가량은 게임 밸런스에 맞게 조정
                    Debug.Log($"{currentSkill.skillName} 강화 성공! 현재 레벨: {currentSkill.level}, Damage: {currentSkill.damage}");
                    break;
            }

            UpdateSkillInfo();
            // SkillUIManager의 RefreshSkillUI 메서드 호출
            if (skillUIManager != null)
            {
                skillUIManager.RefreshSkillUI();
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInfoPanel : MonoBehaviour
{
    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;

    public Button equipButton;
    private SkillDataSO currentSkill;

    public void SetSkillInfo(SkillDataSO skill)
    {
        currentSkill = skill;
        skillIcon.sprite = skill.icon;
        skillName.text = skill.skillName;
        skillDescription.text = skill.description;
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
}

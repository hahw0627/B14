using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneSkillManager : MonoBehaviour
{
    public SkillManager skillManager;
    public List<Button> skillButtons;

    private void Start()
    {
        UpdateSkillButtons();
    }
    public void UpdateSkillButtons()
    {
        List<SkillDataSO> equippedSkills = skillManager.equippedSkills;

        for(int i = 0; i < skillButtons.Count; i++)
        {
            if(i<equippedSkills.Count)
            {
                SetupSkillButton(skillButtons[i], equippedSkills[i]);
            }
            else
            {
                DisableSkillButton(skillButtons[i]);
            }
        }
    }
    private void SetupSkillButton(Button button, SkillDataSO skill)
    {
        button.gameObject.SetActive(true);
        button.image.sprite = skill.icon;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => UseSkill(skill));

        // ��ٿ� ǥ�� �� �߰� ���
    }
    
    private void DisableSkillButton(Button button)
    {
        button.gameObject.SetActive(false);
    }

    private void UseSkill(SkillDataSO skill)
    {
        // ���⿡ ��ų ��� ����
        Debug.Log($"Using skill : {skill.skillName}");
    }
}

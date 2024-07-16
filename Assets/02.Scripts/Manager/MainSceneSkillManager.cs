using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneSkillManager : MonoBehaviour
{
    public SkillManager skillManager;
    public List<Button> skillButtons;
    public AutoSkillManager autoSkillManager;
    public List<Image> cooldownImages;
    public List<Text> cooldownTexts;

    private Dictionary<SkillDataSO, Coroutine> cooldownCoroutines = new Dictionary<SkillDataSO, Coroutine>();

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
                SetupSkillButton(skillButtons[i], cooldownImages[i], cooldownTexts[i], equippedSkills[i]);
            }
            else
            {
                DisableSkillButton(skillButtons[i], cooldownImages[i], cooldownTexts[i]);
            }
        }
    }
    private void SetupSkillButton(Button button, Image cooldownImage, Text cooldownText, SkillDataSO skill)
    {
        button.gameObject.SetActive(true);
        button.image.sprite = skill.icon;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => UseSkill(skill));

        // ��ٿ� �̹��� �ʱ�ȭ
        cooldownImage.gameObject.SetActive(true);
        cooldownImage.fillAmount = 0f;

        cooldownText.gameObject.SetActive(true);
        cooldownText.text = "";

        if (cooldownCoroutines.TryGetValue(skill, out Coroutine existingCoroutine))
        {
            if(existingCoroutine != null)
            {
                StopCoroutine(existingCoroutine);
            }
            cooldownCoroutines.Remove(skill);
        }

        // �� �ڷ�ƾ ���� �� ��ųʸ��� �߰�
        Coroutine newCoroutine = StartCoroutine(UpdateCooldown(cooldownImage, cooldownText, skill));
        cooldownCoroutines[skill] = newCoroutine;   
    }
    
    private void DisableSkillButton(Button button, Image cooldownImage, Text cooldownText)
    {
        button.gameObject.SetActive(false);
        cooldownImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    public void UseSkill(SkillDataSO skill)
    {
        if (autoSkillManager.GetSkillCooldown(skill) <= 0)
        {
            Debug.Log($"Using skill : {skill.skillName}");
            autoSkillManager.SetSkillOnCooldown(skill);

            // ��Ÿ�� �ð�ȭ �ڷ�ƾ �����
            int index = skillManager.equippedSkills.IndexOf(skill);
            if(index != -1 && index < cooldownImages.Count)
            {
                // ���� �ڷ�ƾ�� �ִٸ� ����
                if (cooldownCoroutines.TryGetValue(skill, out Coroutine existingCoroutine))
                {
                    if(existingCoroutine != null)
                    {
                        StopCoroutine(existingCoroutine);
                    }
                    cooldownCoroutines.Remove(skill);
                }

                // �� �ڷ�ƾ ���� �� ��ųʸ��� �߰�
                Coroutine newCoroutine = StartCoroutine(UpdateCooldown(cooldownImages[index], cooldownTexts[index], skill));
                cooldownCoroutines[skill] = newCoroutine;
            }
        }
        else
        {
            Debug.Log($"Skill {skill.skillName} is on cooldown");
        }
    }

    private IEnumerator UpdateCooldown(Image cooldownImage, Text cooldownText, SkillDataSO skill)
    {
        while (true)
        {
            float remainingCooldown = autoSkillManager.GetSkillCooldown(skill);
            float totalCooldown = skill.cooldown;
            if (remainingCooldown > 0)
            {
                cooldownImage.fillAmount = remainingCooldown / totalCooldown;
                cooldownText.text = remainingCooldown.ToString("F0");
            }
            else
            {
                cooldownImage.fillAmount = 0f;
                cooldownText.text = "";
                yield break; //��Ÿ���� ������ �ڷ�ƾ ����
            }

            yield return null; // �� �����Ӹ��� ������Ʈ
        }
    }
}

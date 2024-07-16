using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AutoSkillManager : MonoBehaviour
{
    public MainSceneSkillManager mainSceneSkillManager;
    public float autoUseInterval = 0.5f; // 스킬 사용 체크 간격
    private bool isAutoMode = false;
    private Coroutine autoUseCoroutine;
    public Button autoButton;
    public Color autoOnColor = Color.white;
    public Color autoOffColor = Color.gray;

    private Dictionary<SkillDataSO, float> skillCooldowns = new Dictionary<SkillDataSO, float>();

    private void Update()
    {
        foreach(var key in skillCooldowns.Keys.ToList())
        {
            skillCooldowns[key] = Mathf.Max(0, skillCooldowns[key] - Time.deltaTime);
        }
    }

    public void ToggleAutoMode()
    {
        isAutoMode = !isAutoMode;
        if (isAutoMode)
        {
            StartAutoUse();
        }
        else
        {
            StopAutoUse();
        }
        UpdateAutoButtonVisual();
    }

    private void UpdateAutoButtonVisual()
    {
        if (autoButton != null)
        {
            autoButton.image.color = isAutoMode ? autoOnColor : autoOffColor;
        }
    }

    private void StartAutoUse()
    {
        if(autoUseCoroutine == null)
        {
            autoUseCoroutine = StartCoroutine(AutoUseSkills());
        }
    }

    private void StopAutoUse()
    {
        if(autoUseCoroutine != null)
        {
            StopCoroutine(autoUseCoroutine);
            autoUseCoroutine = null;
        }
    }

    private IEnumerator AutoUseSkills()
    {
        while (isAutoMode)
        {
            List<SkillDataSO> equippedSkills = mainSceneSkillManager.skillManager.equippedSkills;
            foreach (SkillDataSO skill in equippedSkills)
            {
                if(!skillCooldowns.ContainsKey(skill) || skillCooldowns[skill] <= 0)
                {
                    mainSceneSkillManager.UseSkill(skill);
                }
            }
            yield return new WaitForSeconds(autoUseInterval);
        }
    }
    public void SetSkillOnCooldown(SkillDataSO skill)
    {
        skillCooldowns[skill] = skill.cooldown;
    }

    public float GetSkillCooldown(SkillDataSO skill)
    {
        if (skillCooldowns.TryGetValue(skill, out float cooldown))
        {
            return Mathf.Max(0, cooldown);
        }
        return 0f;
    }
}

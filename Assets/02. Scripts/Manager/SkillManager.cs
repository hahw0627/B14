using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public List<SkillDataSO> equippedSkills = new List<SkillDataSO>();
    public int maxEquippedSkills = 5;
    public event Action OnEquippedSkillsChanged;

    [Header("Auto Skill Management")]
    public MainSceneSkillManager mainSceneSkillManager;
    public float autoUseInterval = 0.5f;
    public Button autoButton;
    public Color autoOnColor = Color.white;
    public Color autoOffColor = Color.gray;
    public Player player;

    private bool isAutoMode;
    private Coroutine autoUseCoroutine;
    private Dictionary<SkillDataSO, float> skillCooldowns = new Dictionary<SkillDataSO, float>();

    private void Awake()
    {
        UpdateEquippedSkills();
        OnEquippedSkillsChanged?.Invoke();
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    public void UpdateEquippedSkills()
    {
        equippedSkills.Clear();
        if (DataManager.Instance.PlayerDataSo.Skills != null)
        {
            equippedSkills.AddRange(DataManager.Instance.PlayerDataSo.Skills);
        }
    }

    public void EquipSkill(SkillDataSO skill)
    {
        if (equippedSkills.Count < maxEquippedSkills && !equippedSkills.Contains(skill) &&
            DataManager.Instance.AllSkillsDataSo.Contains(skill))
        {
            equippedSkills.Add(skill);
            OnEquippedSkillsChanged?.Invoke();
        }
    }

    public void UnequipSkill(SkillDataSO skill)
    {
        int index = equippedSkills.IndexOf(skill);
        if (index != -1)
        {
            equippedSkills[index] = null;
            OnEquippedSkillsChanged?.Invoke();
        }
    }

    public void ReplaceSkill(int index, SkillDataSO newSkill)
    {
        if (index >= 0 && index < equippedSkills.Count)
        {
            equippedSkills[index] = newSkill;
            OnEquippedSkillsChanged?.Invoke();
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
        if (autoUseCoroutine == null)
        {
            autoUseCoroutine = StartCoroutine(AutoUseSkills());
        }
    }

    private void StopAutoUse()
    {
        if (autoUseCoroutine != null)
        {
            StopCoroutine(autoUseCoroutine);
            autoUseCoroutine = null;
        }
    }

    private IEnumerator AutoUseSkills()
    {
        while (isAutoMode)
        {
            if (player.Scanner.nearestTarget != null)
            {
                SkillDataSO skillToUse = FindUsableSkill();
                if (skillToUse != null)
                {
                    int skillIndex = equippedSkills.IndexOf(skillToUse);
                    if (skillIndex != -1)
                    {
                        mainSceneSkillManager.UseSkill(skillIndex);
                    }
                    yield return new WaitForSeconds(1.0f);
                }
                else
                {
                    yield return new WaitForSeconds(autoUseInterval);
                }
            }
            else
            {
                yield return new WaitForSeconds(autoUseInterval);
            }
        }
    }


    private SkillDataSO FindUsableSkill()
    {
        return equippedSkills.Find(skill => !skillCooldowns.ContainsKey(skill) || skillCooldowns[skill] <= 0);
    }

    public void SetSkillOnCooldown(SkillDataSO skill)
    {
        skillCooldowns[skill] = skill.Cooldown;
    }

    public float GetSkillCooldown(SkillDataSO skill)
    {
        return skillCooldowns.TryGetValue(skill, out float cooldown) ? Mathf.Max(0, cooldown) : 0f;
    }

    private void UpdateCooldowns()
    {
        float deltaTime = Time.deltaTime;
        var updatedCooldowns = new Dictionary<SkillDataSO, float>();

        foreach (var pair in skillCooldowns)
        {
            float newCooldown = Mathf.Max(0, pair.Value - deltaTime);
            if (newCooldown > 0)
            {
                updatedCooldowns[pair.Key] = newCooldown;
            }
        }

        skillCooldowns = updatedCooldowns;
    }

    public void EquipSkillAtIndex(int index, SkillDataSO newSkill)
    {
        if (index < 0 || index >= maxEquippedSkills)
        {
            Debug.LogWarning("Invalid index for equipping skill.");
            return;
        }

        if (!DataManager.Instance.AllSkillsDataSo.Contains(newSkill))
        {
            Debug.LogWarning("Skill not found in allSkillsDataSO.");
            return;
        }

        // �� ��ų�� �̹� �����Ǿ� �ִ��� Ȯ��
        int existingIndex = equippedSkills.FindIndex(s => s != null && s.SkillName == newSkill.SkillName);

        // �̹� ������ ��ų�� �ְ�, �� ��ġ�� ���� �����Ϸ��� ��ġ�� �ٸ��ٸ�
        if (existingIndex != -1 && existingIndex != index)
        {
            // �̹� ������ ��ų�� ����
            equippedSkills[existingIndex] = null;
        }

        // �� ��ų�� ������ ��ġ�� �̹� �ٸ� ��ų�� �ִٸ� ����
        if (index < equippedSkills.Count && equippedSkills[index] != null)
        {
            equippedSkills[index] = null;
        }

        // �� ��ų ����
        while (equippedSkills.Count <= index)
        {
            equippedSkills.Add(null);
        }

        equippedSkills[index] = newSkill;
        OnEquippedSkillsChanged?.Invoke();
    }
}

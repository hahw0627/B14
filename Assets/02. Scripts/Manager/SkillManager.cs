using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillDataSO> allSkills;
    public List<SkillDataSO> equippedSkills;
    public int maxEquippedSkills = 5;

    public event System.Action OnEquippedSkillsChanged;

    public void EquipSkill(SkillDataSO skill)
    {
        if(equippedSkills.Count < maxEquippedSkills && !equippedSkills.Contains(skill))
        {
            equippedSkills.Add(skill);
            OnEquippedSkillsChanged?.Invoke();
        }
    }

    public void UnequipSkill(SkillDataSO skill)
    {
        if (equippedSkills.Remove(skill))
        {
            OnEquippedSkillsChanged?.Invoke();
        }
    }
}

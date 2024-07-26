using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillDataSO> allSkills;
    public List<SkillDataSO> equippedSkills;
    public PlayerDataSO playerDataSO;

    public int maxEquippedSkills = 5;

    public event System.Action OnEquippedSkillsChanged;

    private void Awake()
    {
        UpdateEquippedSkills();
        OnEquippedSkillsChanged?.Invoke();
    }

    public void UpdateEquippedSkills() //playerdataSO에 있는 skill equipped스킬에 넣기    
    {
      

        if (playerDataSO.skills == null)
            return;

        foreach (var skill in playerDataSO.skills)
        {
            equippedSkills.Add(skill);
        }
    }

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

    public void ReplaceSkill(int index, SkillDataSO newskill)
    {
        if(index >= 0 && index < equippedSkills.Count)
        {
            equippedSkills[index] = newskill;
            OnEquippedSkillsChanged?.Invoke();
        }
    }
}

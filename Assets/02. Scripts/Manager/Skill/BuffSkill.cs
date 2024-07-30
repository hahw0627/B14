using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : MonoBehaviour
{
    private SkillDataSO skillData;
    private Player player;

    public void Initialize(SkillDataSO skill, Player targetPlayer)
    {
        skillData = skill;
        player = targetPlayer;
        ApplyBuff();
    }

    private void ApplyBuff()
    {
        switch (skillData.skillType)
        {
            case Define.SkillType.AttackBuff:
                ApplyAttackBuff();
                break;
            case Define.SkillType.HealBuff:
                ApplyHealBuff();
                break;
            default:
                Debug.LogWarning("Unsupported buff type");
                break;
        }
    }

    private void ApplyAttackBuff()
    {
        Debug.Log($"BuffSkill applying attack buff: {skillData.buffAmount}");
        player.ApplyAttackBuff(skillData.buffAmount);

    }

    private void ApplyHealBuff()
    {
        
        player.Heal(skillData.buffAmount);
        Debug.Log($"Applied heal buff Healed for : {skillData.buffAmount}");
    }
}

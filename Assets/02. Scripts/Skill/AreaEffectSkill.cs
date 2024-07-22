using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaEffectSkill : MonoBehaviour
{
    private SkillDataSO skillData;
    private int playerBaseDamage;
    private float damageInterval = 0.5f; // 데미지를 주는 간격 (초)

    public void Initialize(SkillDataSO skill, int playerDamage)
    {
        skillData = skill;
        playerBaseDamage = playerDamage;
        StartCoroutine(DealDamageOverTime());    
    }

    private IEnumerator DealDamageOverTime()
    {
        float elapsedTime = 0f;
        while (elapsedTime < skillData.duration)
        {
            ApplyDamageToEnemiesInRange();
            yield return new WaitForSeconds(damageInterval);
            elapsedTime += damageInterval;
        }
    }

    private void ApplyDamageToEnemiesInRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, skillData.aoeRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Monster"))
            {
                ApplyDamage(hitCollider.transform);
            }
        }
    }

    private void ApplyDamage(Transform target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if(damageable!= null)
        {
            int totalDamage = skillData.damage + playerBaseDamage;
            Debug.Log($"Area effect skill '{skillData.skillName}' hit monster '{target.name}'. Applying damage: {totalDamage} (Skill : {skillData.damage}, Player Base : {playerBaseDamage})");
            damageable.TakeDamage(totalDamage);

        }
        else
        {
            Debug.LogWarning($"Monster '{target.name}' does not implement IDamageable interface");
        }
    }

    private void OnDrawGizmos()
    {
        if(skillData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, skillData.aoeRadius);
        }
    }
}

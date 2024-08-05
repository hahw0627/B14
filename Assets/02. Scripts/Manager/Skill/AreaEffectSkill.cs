using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaEffectSkill : MonoBehaviour
{
    private SkillDataSO skillData;
    private float damageInterval = 0.5f; // 데미지를 주는 간격 (초)

    public void Initialize(SkillDataSO skill)
    {
        skillData = skill;
        StartCoroutine(DealDamageOverTime());
        StartCoroutine(CameraShake.Instance.Shake(0.5f, 0.2f));
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
            int totalDamage = skillData.damage + DataManager.Instance.playerDataSO.Damage;
            Debug.Log($"Area effect skill '{skillData.skillName}' hit monster '{target.name}'. Applying damage: {totalDamage} (Skill : {skillData.damage}, Player Base : {DataManager.Instance.playerDataSO.Damage})");
            damageable.TakeDamage(totalDamage, true);

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

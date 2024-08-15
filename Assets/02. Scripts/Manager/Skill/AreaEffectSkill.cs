using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaEffectSkill : MonoBehaviour
{
    private SkillDataSO skillData;
    private float damageInterval = 0.5f; // �������� �ִ� ���� (��)

    public void Initialize(SkillDataSO skill)
    {
        skillData = skill;
        StartCoroutine(DealDamageOverTime());
        StartCoroutine(ApplyEffects());
    }

    private IEnumerator ApplyEffects()
    {
        // 카메라 흔들림 효과 적용
        StartCoroutine(CameraShake.Instance.Shake(0.5f, 0.2f));

        
        /*
        // 기기 진동 적용
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.Vibrate();
        }
        */
        
        yield return null;
    }


    private IEnumerator DealDamageOverTime()
    {
        float elapsedTime = 0f;
        while (elapsedTime < skillData.Duration)
        {
            ApplyDamageToEnemiesInRange();
            yield return new WaitForSeconds(damageInterval);
            elapsedTime += damageInterval;
        }
    }

    private void ApplyDamageToEnemiesInRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, skillData.AoeRadius);
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
        if (damageable != null)
        {
            int totalDamage = skillData.Damage + DataManager.Instance.PlayerDataSo.Damage;
            Debug.Log(
                $"Area effect skill '{skillData.SkillName}' hit monster '{target.name}'. Applying damage: {totalDamage} (Skill : {skillData.Damage}, Player Base : {DataManager.Instance.PlayerDataSo.Damage})");
            damageable.TakeDamage(totalDamage, true);
        }
        else
        {
            Debug.LogWarning($"Monster '{target.name}' does not implement IDamageable interface");
        }
    }

    private void OnDrawGizmos()
    {
        if (skillData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, skillData.AoeRadius);
        }
    }
}
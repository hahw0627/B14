using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    public SkillDataSO skillData { get; private set; }
    private Vector3 targetPosition;
    private float speed;
    private ParticleSystem particleSystem;
    private TrailRenderer trailRenderer;

    public void Initialize(SkillDataSO skill, Vector3 target)
    {
        skillData = skill;
        targetPosition = target;
        speed = skillData.projectileSpeed;

        // 즉시 목표를 향해 회전
        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem == null)
        {
            Debug.LogWarning("Particle system not found on projectile!");
        }
        else
        {
            var main = particleSystem.main;
            main.simulationSpeed = speed;
        }

        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer == null)
        {
            Debug.LogWarning("TrailRenderer not found on projectile!");
        }

        StartCoroutine(MoveProjectile());
    }

    private IEnumerator MoveProjectile()
    {
        float journeyLength = Vector3.Distance(transform.position, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, targetPosition, fractionOfJourney);

            // 파티클 시스템 위치 업데이트
            if (particleSystem != null)
            {
                particleSystem.transform.position = transform.position;
            }

            // 충돌 체크
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    ApplyDamage(hitCollider.transform);
                    Destroy(gameObject);
                    yield break;
                }
            }

            yield return null;
        }

        // 목표 지점에 도달하면 프로젝타일을 파괴
        Destroy(gameObject);
    }

    private void ApplyDamage(Transform target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            int totalDamage = skillData.damage + DataManager.Instance.playerDataSO.Damage;
            Debug.Log($"Skill projectile '{skillData.skillName}' hit monster '{target.name}'. Applying damage: {totalDamage} (Skill: {skillData.damage}, Player Base: {DataManager.Instance.playerDataSO.Damage})");
            damageable.TakeDamage(totalDamage, true);
        }
        else
        {
            Debug.LogWarning($"Monster '{target.name}' does not implement IDamageable interface");
        }
    }

    private void OnDestroy()
    {
        // TrailRenderer가 있다면 자연스럽게 사라지도록 설정
        if (trailRenderer != null)
        {
            trailRenderer.transform.SetParent(null);
            Destroy(trailRenderer.gameObject, trailRenderer.time);
        }
    }
}

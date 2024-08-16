using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
        speed = skillData.ProjectileSpeed;

        // ��� ��ǥ�� ���� ȸ��
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

            // ��ƼŬ �ý��� ��ġ ������Ʈ
            if (particleSystem != null)
            {
                particleSystem.transform.position = transform.position;
            }

            // �浹 üũ
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

        // ��ǥ ������ �����ϸ� ������Ÿ���� �ı�
        Destroy(gameObject);
    }

    private void ApplyDamage(Transform target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            var totalDamage = skillData.Damage + DataManager.Instance.PlayerDataSo.Damage;
            Debug.Log($"Skill projectile '{skillData.SkillName}' hit monster '{target.name}'. Applying damage: {totalDamage} (Skill: {skillData.Damage}, Player Base: {DataManager.Instance.PlayerDataSo.Damage})");
            damageable.TakeDamage(totalDamage, true);
        }
        else
        {
            Debug.LogWarning($"Monster '{target.name}' does not implement IDamageable interface");
        }
    }

    private void OnDestroy()
    {
        // TrailRenderer�� �ִٸ� �ڿ������� ��������� ����
        if (trailRenderer != null)
        {
            trailRenderer.transform.SetParent(null);
            Destroy(trailRenderer.gameObject, trailRenderer.time);
        }
    }
}

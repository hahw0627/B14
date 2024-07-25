using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public Player player;
    public int damage;
    public int speed = 3;
    private Vector3 direction;

    private void Start()
    {
        // target이 null이 아니면 방향을 설정, null이면 현재 방향 유지
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }

        // 3초 후에 삭제
        StartCoroutine(DestroyAfterTime(1.5f));
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime; // 설정된 방향으로 투사체 이동
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        // 오브젝트 풀링으로 구현 예정 1
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("투사체 충돌 감지!");
        if (collision.CompareTag("Monster"))
        {
            // 오브젝트 풀링으로 구현 예정 1
            Destroy(gameObject);
            AttackMonster(collision.transform);
        }
    }

    private void AttackMonster(Transform target)
    {
        if (target != null)
        {
            // 타겟이 Monster_Test 컴포넌트를 가지고 있는지 확인하여 TakeDamage 호출
            Monster123 monsterScript = target.GetComponent<Monster123>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage(damage);
            }
            // 만약 Boss 스크립트를 가진 오브젝트도 데미지를 받아야 한다면
            else
            {
                Boss bossScript = target.GetComponent<Boss>();
                if (bossScript != null)
                {
                    bossScript.TakeDamage(damage);
                }
            }
        }
    }
}

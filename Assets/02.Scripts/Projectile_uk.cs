using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_uk : MonoBehaviour
{
    public Transform target;
    public Player player;
    public int damage;
    public int speed = 3;

    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        if (gameObject.activeSelf) // 투사체를 오브젝트 풀링으로 구현하게 될 경우를 대비하여 활성화 상태 확인
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized; // 목표 방향 벡터
                transform.position += direction * speed * Time.deltaTime; // 투사체 이동
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("투사체 충돌 감지!");
        if (collision.CompareTag("Monster"))    // Monster태그이면 실행
        {
            Destroy(gameObject);  // 오브젝트 풀링으로 변경 시, SetActive(false)로 변경 필요
            AttackMonster(collision.transform);  // 데미지 주기  ( target 대신 collision.transform을 사용한 이유 : 다른 오브젝트와 충돌해도 target에게 데미지가 들어가는 문제 )
        }
    }

    private void AttackMonster(Transform monster)
    {
        if (monster != null)
        {
            Monster_Test monsterScript = monster.GetComponent<Monster_Test>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage(damage);
            }
        }
    }
}

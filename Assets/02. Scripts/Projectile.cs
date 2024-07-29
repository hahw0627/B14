using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public int damage;
    public int speed = 3;
    private Vector3 direction;
    public string shooterTag; // 투사체를 발사한 주체의 태그 (Player 또는 Monster)
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        // target이 null이 아니면 방향을 설정, null이면 현재 방향 유지
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
    }

    private void OnEnable()
    {
        // 3초 후에 삭제
        StartCoroutine(DestroyAfterTime(1.5f));

    }

    private void Update()
    {
        // 설정된 방향으로 투사체 이동
        transform.position += direction * speed * Time.deltaTime;
    }

    // 일정 시간 이후 비활성화
    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((shooterTag == "Player" && collision.CompareTag("Monster")) ||
            (shooterTag == "Monster" && collision.CompareTag("Player")))
        {
            // 다른 팀일 경우에만 공격
            if (collision.CompareTag("Player"))
            {
                Player player = collision.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
            else if (collision.CompareTag("Monster"))
            {
                Monster123 monster = collision.GetComponent<Monster123>();
                if (monster != null)
                {
                    monster.TakeDamage(damage);
                }
            }
            ProjectilePool.Instance.ReturnProjectile(gameObject);
        }
    }

    public void SetDirection(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}


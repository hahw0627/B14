using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public int damage;
    public int speed = 3;
    private Vector3 direction;
    public string shooterTag; // ����ü�� �߻��� ��ü�� �±� (Player �Ǵ� Monster)
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        // target�� null�� �ƴϸ� ������ ����, null�̸� ���� ���� ����
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
    }

    private void OnEnable()
    {
        // 3�� �Ŀ� ����
        StartCoroutine(DestroyAfterTime(1.5f));

    }

    private void Update()
    {
        // ������ �������� ����ü �̵�
        transform.position += direction * speed * Time.deltaTime;
    }

    // ���� �ð� ���� ��Ȱ��ȭ
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
            // �ٸ� ���� ��쿡�� ����
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


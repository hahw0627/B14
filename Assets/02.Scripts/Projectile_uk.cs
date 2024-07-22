using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_uk : MonoBehaviour
{
    public Transform target;
    public Player player;
    public int damage;
    public int speed = 3;
    private Vector3 direction;

    private void Start()
    {
        // target�� null�� �ƴϸ� ������ ����, null�̸� ���� ���� ����
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }

        // 3�� �Ŀ� ����
        StartCoroutine(DestroyAfterTime(1f));
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime; // ������ �������� ����ü �̵�
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("����ü �浹 ����!");
        if (collision.CompareTag("Monster"))    // Monster�±��̸� ����
        {
            Destroy(gameObject);  // ������Ʈ Ǯ������ ���� ��, SetActive(false)�� ���� �ʿ�
            AttackMonster(collision.transform);  // ������ �ֱ�  ( target ��� collision.transform�� ����� ���� : �ٸ� ������Ʈ�� �浹�ص� target���� �������� ���� ���� )
        }
    }

    private void AttackMonster(Transform target)
    {
        if (target != null)
        {
            // Ÿ���� Monster_Test ������Ʈ�� ������ �ִ��� Ȯ���Ͽ� TakeDamage ȣ��
            Monster_Test monsterScript = target.GetComponent<Monster_Test>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage(damage);
            }
            // ���� Boss ��ũ��Ʈ�� ���� ������Ʈ�� �������� �޾ƾ� �Ѵٸ�
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

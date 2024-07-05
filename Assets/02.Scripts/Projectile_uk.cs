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
        if (gameObject.activeSelf) // ����ü�� ������Ʈ Ǯ������ �����ϰ� �� ��츦 ����Ͽ� Ȱ��ȭ ���� Ȯ��
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized; // ��ǥ ���� ����
                transform.position += direction * speed * Time.deltaTime; // ����ü �̵�
            }
            else
            {
                Destroy(gameObject);
            }
        }
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

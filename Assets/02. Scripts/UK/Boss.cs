using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable
{
    public MonsterDataSO_Test monsterData;
    public GameObject monsterProjectilePrefab;
    public Transform target;

    public int Hp;
    public int damage;
    public float attackSpeed;
    private bool isAttacking = false;
    public MonsterSpawner_UK monsterSpawner;
    private float moveTime = 0.0f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Hp = monsterData.Hp * 3; // ���� ���ʹ� HP�� 2��� ����
        damage = monsterData.Damage * 2; // ���� ���ʹ� �������� 2��� ����
        attackSpeed = monsterData.AttackSpeed;
        moveTime = 0.0f; // moveTime �ʱ�ȭ
        isAttacking = false;
    }

    private void Update()
    {
        // ���� �̵� ����
        if (moveTime < 1.5f)
        {
            spriteRenderer.flipX = true;
            transform.Translate(Vector3.left * 2.0f * Time.deltaTime);
            moveTime += Time.deltaTime;
        }
        else if (moveTime >= 1.5f)
        {
            animator.SetBool("IsBattle", true);
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        while (true)
        {
            if (target != null)
            {
                GameObject projectile = Instantiate(monsterProjectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<MonsterProjectile_uk>().target = target;
                projectile.GetComponent<MonsterProjectile_uk>().damage = damage;
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("���� ���� HP ����\n" + "HP : " + Hp + " / ������ : " + damage);

        if (Hp <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("���� ���� ��Ȱ��ȭ");
            if (monsterSpawner != null)
            {
                monsterSpawner.BossDeath();
            }
            else
            {
                Debug.LogWarning("MonsterSpawner_UK�� �Ҵ���� �ʾҽ��ϴ�.");
            }
        }
    }
}

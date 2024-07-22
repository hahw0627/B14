using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerDataSO playerData;
    public int damage;
    public float attackSpeed;
    public int currentHp;
    public GameObject projectilePrefab;
    public MonsterSpawner_UK monsterSpawner;
    private Scanner scanner;
    private Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer ���� �߰�

    private void Awake()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO ���� ����");
            return;
        }

        damage = playerData.Damage;
        attackSpeed = playerData.AttackSpeed;
        currentHp = playerData.Hp;

        scanner = GetComponent<Scanner>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(Attack());
        StartCoroutine(RecoverHp());
    }

    private void Update()
    {
        if (scanner.nearestTarget != null)
        {
            animator.SetBool("IsBattle", true);
        }
        else
        {
            animator.SetBool("IsBattle", false);
        }
    }


    private IEnumerator Attack()
    {
        while (true)
        {
            // scanner�� nearestTarget�� null�� �ƴ� ��쿡�� nearestTarget�� ������ target���� ���� + ����ü ����
            if (scanner.nearestTarget != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile_uk>().target = scanner.nearestTarget;   // ������ ����ü�� Ÿ�� ����
                projectile.GetComponent<Projectile_uk>().damage = this.damage;   // ������ ����ü�� ������ ����
                projectile.GetComponent<Projectile_uk>().player = this; // ������ ����ü�� �÷��̾� ����
            }

            yield return new WaitForSeconds(1 / attackSpeed); // 1�ʿ� / attackSpeed ��ŭ ����
        }
    }

    private IEnumerator RecoverHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (currentHp < playerData.Hp)
            {
                currentHp += playerData.HpRecovery;
                if (currentHp > playerData.Hp)
                {
                    currentHp = playerData.Hp;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Player HP ����! : {currentHp} - {damage}");


        if (currentHp <= 0)
        {
            if (monsterSpawner != null)
            {
                monsterSpawner.stagePage = 0;
            }
            else
            {
                Debug.LogError("MonsterSpawner_UK ���� ����.");
            }

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                monster.SetActive(false);
            }

            Debug.Log("�÷��̾� ���. ���� ��Ȱ��ȭ. �������� �ʱ�ȭ");

            StartCoroutine(PlayerDeath()); // ü�� 0 ������ �� ���� ���� �ڷ�ƾ ����
        }
    }

    private IEnumerator PlayerDeath()
    {
        // 1�� ���� 2�� 255���� 70���� �����ϰ� �ٽ� 255���� ����
        for (int i = 0; i < 2; i++)
        {
            // 255���� 70���� ����
            for (float alpha = 1.0f; alpha >= 0.275f; alpha -= Time.deltaTime * 2)
            {
                SetSpriteAlpha(alpha);
                yield return null;
            }
            // 70���� 255���� ����
            for (float alpha = 0.275f; alpha <= 1.0f; alpha += Time.deltaTime * 2)
            {
                SetSpriteAlpha(alpha);
                yield return null;
            }
        }

        // ü�� �� �������� �ʱ�ȭ
        currentHp = playerData.Hp;
        Debug.Log("�÷��̾� ü�� ����");
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}

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
    public Scanner scanner;
    private Animator animator;
    public SpriteRenderer spriteRenderer;
    private bool isUsingSkill = false;
    private Coroutine attackCoroutine;
    public int CurrentDamage { get; private set; }

    private void Awake()
    {
        UpdateDamage();

        damage = playerData.Damage;
        attackSpeed = playerData.AttackSpeed;
        currentHp = playerData.Hp;

        scanner = GetComponent<Scanner>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartAttacking();
        StartCoroutine(RecoverHp());
    }

    public void StartAttacking()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(Attack());
        }
    }

    public void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private void Update()
    {
        if (scanner.nearestTarget != null)
        {
            // target�� �����ϸ� IsBattel�� true�� ����
            animator.SetBool("IsBattle", true);
            Debug.Log("��Ʋ ����");
        }
        else
        {
            // target�� null�̸� IsBattel�� false�� ����
            animator.SetBool("IsBattle", false);
            Debug.Log("��Ʋ ����");
        }
    }


    private IEnumerator Attack()
    {
        while (true)
        {
            // scanner�� nearestTarget�� null�� �ƴ� ��쿡�� nearestTarget�� ������ target���� ���� + ����ü ����
            if (!isUsingSkill && scanner.nearestTarget != null)
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

    public void SetUsingSkill(bool usingSkill)
    {
        isUsingSkill = usingSkill;
    }

    public void ApplyAttackBuff(int amount)
    {
        Debug.Log($"Applying attack buff : {amount}");
        playerData.Damage += amount;
        UpdateDamage();
    }
    private void UpdateDamage()
    {
        Debug.Log($"Updating damage. Base: {playerData.Damage}, Buff: {playerData.Damage}");
        CurrentDamage = playerData.Damage;
        Debug.Log($"New damage: {CurrentDamage}");
    }

    public void Heal(int amount) // ���� �÷��̾� �ǰ� ������ ���� ����
    {

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

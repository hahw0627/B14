using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerDataSO playerData;
    public MonsterSpawner monsterSpawner;
    public Scanner scanner;

    private Coroutine attackCoroutine;
    public Transform fireMuzzle;

    public int damage;
    public float attackSpeed;
    public int currentHp;
    private bool isUsingSkill = false;
    public int CurrentDamage { get; private set; }

    private void Awake()
    {
        playerData = DataManager.Instance.playerDataSO;
        scanner = GetComponent<Scanner>();

        attackSpeed = playerData.AttackSpeed;
        currentHp = playerData.Hp;
        UpdateDamage();
    }

    private void Start()
    {
        StartAttacking();
        StartCoroutine(RecoverHp());
    }

    // ���ݱ��
    private IEnumerator Attack()
    {
        while (true)
        {
            // scanner�� nearestTarget�� null�� �ƴ� ��쿡�� nearestTarget�� ������ target���� ���� + ����ü ����
            if (!isUsingSkill && scanner.nearestTarget != null)
            {
                GameObject projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = fireMuzzle.position;
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.target = scanner.nearestTarget;   // ������ ����ü�� Ÿ�� ����
                projectileScript.SetDirection(scanner.nearestTarget.transform.position);
                projectileScript.damage = this.CurrentDamage;   // ������ ����ü�� ������ ����
                projectileScript.shooterTag = "Player";
                projectileScript.SetColor(Color.blue);
            }

            yield return new WaitForSeconds(1 / attackSpeed); // 1�ʿ� / attackSpeed ��ŭ ����
        }
    }

    // ü�� ȸ�� ���
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

    // �ǰ� ���
    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            // ���� ��Ȱ��ȭ
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                monster.SetActive(false);
            }

            // �������� ������ �ʱ�ȭ
            monsterSpawner.stagePage = 0;

            // ü�� �ʱ�ȭ
            currentHp = playerData.Hp;
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
        damage = CurrentDamage;
        Debug.Log($"New damage: {CurrentDamage}");
    }

    public void Heal(int amount) // ���� �÷��̾� �ǰ� ������ ���� ����
    {

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

    //private void Update()
    //{
    //    if (scanner.nearestTarget != null)
    //    {
    //        // target�� �����ϸ� IsBattel�� true�� ����
    //        animator.SetBool("IsBattle", true);
    //        Debug.Log("��Ʋ ����");
    //    }
    //    else
    //    {
    //        // target�� null�̸� IsBattel�� false�� ����
    //        animator.SetBool("IsBattle", false);
    //        Debug.Log("��Ʋ ����");
    //    }
    //}
}

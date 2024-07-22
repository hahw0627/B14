using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerDataSO playerData;
    public int damage;
    public float attackSpeed;
    public Scanner scanner;
    public GameObject projectilePrefab;
    private Animator animator;
    private bool isUsingSkill = false;
    private Coroutine attackCoroutine;
    private int currentAttackBuff = 0;
    public int CurrentDamage { get; private set; }

    private void Awake()
    {
        UpdateDamage();
        damage = playerData.Damage;
        attackSpeed = playerData.AttackSpeed;
        scanner = GetComponent<Scanner>();
        animator = GetComponent<Animator>();
        Debug.Log("Damage: " + damage);
        Debug.Log("Attack Speed: " + attackSpeed);
    }

    private void Start()
    {
        StartAttacking();
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
}

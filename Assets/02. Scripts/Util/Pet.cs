using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public CompanionDataSO companionData;
    public Scanner scanner; 
    public Transform projectilPos;
    public float attackSpeed;
    public int damage;
    public Animator animator;

    private void Awake()
    {
        UpdateDamage(companionData.Damage); // �⺻ ���� �� ���
        attackSpeed = companionData.AttackSpeed; // �⺻ ���� �� ���
    }

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        scanner = player.GetComponent<Scanner>();
        animator = GetComponent<Animator>();
        StartCoroutine(Attack());
    }

    public void UpdateDamage(int newDamage)
    {
        damage = newDamage;
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (scanner.NearestTarget != null)
            {
                if (name == "Companion_5(Clone)" || name == "Companion_10(Clone)" || name == "Companion_15(Clone)")
                {
                    animator.SetTrigger("ShotBow");
                }
                else if(name == "Companion_6(Clone)")
                {
                    animator.Play("Fire1H");
                }
                else
                {
                    animator.SetTrigger("Slash1H");
                }
                GameObject projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = projectilPos.position;
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.Target = scanner.NearestTarget.transform;
                projectileScript.SetDirection(scanner.NearestTarget.transform.position);
                projectileScript.Damage = this.damage;
                projectileScript.ShooterTag = "Player";
                projectileScript.SetColor(Color.yellow);
                Monster monster = scanner.NearestTarget.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(this.damage, false, true);
                }
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }

    private IEnumerator WaitTime(float num)
    {
        yield return new WaitForSeconds(num);
    }
}

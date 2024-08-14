using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public CompanionDataSO companionData;
    public Scanner scanner;
    public Transform fireMuzzle;
    public float attackSpeed;
    public int damage;

    private void Awake()
    {
        UpdateDamage(companionData.Damage); // �⺻ ���� �� ���
        attackSpeed = companionData.AttackSpeed; // �⺻ ���� �� ���
    }

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        scanner = player.GetComponent<Scanner>();
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
            // �÷��̾��� scanner���� nearestTarget�� ������
            if (scanner.nearestTarget != null)
            {
                GameObject projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = fireMuzzle.position;
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.Target = scanner.nearestTarget.transform;
                projectileScript.SetDirection(scanner.nearestTarget.transform.position);
                projectileScript.Damage = this.damage; // �÷��̾��� ������ ���
                projectileScript.ShooterTag = "Player";
                projectileScript.SetColor(Color.yellow);
                Monster monster = scanner.nearestTarget.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(this.damage, false, true);  // isPetAttack을 true로 설정
                }
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }
}

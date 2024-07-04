using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerDataSO playerData;
    public int damage;
    public float attackSpeed;
    public Transform target;
    private Scanner scanner;
    public GameObject projectilePrefab;

    private void Awake()
    {
        damage = playerData.Damage;
        attackSpeed = playerData.AttackSpeed;
        scanner = GetComponent<Scanner>();
        Debug.Log("Damage: " + damage);
        Debug.Log("Attack Speed: " + attackSpeed);
    }

    private void Start()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / attackSpeed); // 1�ʿ� / attackSpeed ��ŭ ����

            // scanner�� null�� �ƴ� ��쿡�� nearestTarget�� ������ target���� ����
            if (scanner != null)
            {
                target = scanner.nearestTarget;
            }
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile_uk>().target = this.target;   // ������ ����ü�� Ÿ�� ����
            projectile.GetComponent<Projectile_uk>().damage = this.damage;   // ������ ����ü�� ������ ����
            projectile.GetComponent<Projectile_uk>().player = this; // ������ ����ü�� �÷��̾� ����
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public PetDataSO petData;
    public Scanner scanner;
    public GameObject projectilePrefab;
    public float attackSpeed;
    public int damage;
    public int petNumber;

    private void Awake()
    {
        damage = petData.damage; // �⺻ ���� �� ���
        attackSpeed = petData.attackSpeed; // �⺻ ���� �� ���
    }

    private void Start()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            // �÷��̾��� scanner���� nearestTarget�� ������
            if (scanner.nearestTarget != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().target = scanner.nearestTarget;
                projectile.GetComponent<Projectile>().damage = this.damage; // �÷��̾��� ������ ���
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }
}

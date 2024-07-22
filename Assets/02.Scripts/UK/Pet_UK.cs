using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_UK : MonoBehaviour
{
    public PetDataSO petData;
    public Scanner scanner;
    public GameObject projectilePrefab;
    public float attackSpeed;
    public int damage;
    public int petNumber;

    private void Awake()
    {
        if (petNumber == 1)
        {
            damage = petData.damage; // �⺻ ���� �� ���
        }
        else if (petNumber > 1 && petNumber <= 15)
        {
            damage = (int)(petData.damage * Mathf.Pow(1.5f, petNumber - 1)); // ���� �꺸�� 1.5�� ���� �ɷ�ġ �ο�
        }
        else
        {
            Debug.LogError("Invalid pet number. Pet number must be between 1 and 15.");
            return;
        }

        attackSpeed = petData.attackSpeed; // �÷��̾��� ���� �ӵ� ��������
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
                projectile.GetComponent<Projectile_uk>().target = scanner.nearestTarget;
                projectile.GetComponent<Projectile_uk>().damage = this.damage; // �÷��̾��� ������ ���
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }
}

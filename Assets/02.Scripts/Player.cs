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
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / attackSpeed); // 1�ʿ� / attackSpeed ��ŭ ����

            // scanner�� null�� �ƴ� ��쿡�� nearestTarget�� ������ target���� ����
            if (scanner != null)
            {
                target = scanner.nearestTarget;
            }

            AttackMonster(target);
        }
    }

    private void AttackMonster(Transform monster)
    {
        if (monster != null)
        {
            Monster_Test monsterScript = monster.GetComponent<Monster_Test>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("Monster_Test ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }
}

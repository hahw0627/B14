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
            yield return new WaitForSeconds(1 / attackSpeed); // 1초에 / attackSpeed 만큼 공격

            // scanner가 null이 아닌 경우에만 nearestTarget을 가져와 target으로 설정
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
                Debug.LogError("Monster_Test 스크립트를 찾을 수 없습니다.");
            }
        }
    }
}

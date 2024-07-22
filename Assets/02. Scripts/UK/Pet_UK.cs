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
            damage = petData.damage; // 기본 설정 값 사용
        }
        else if (petNumber > 1 && petNumber <= 15)
        {
            damage = (int)(petData.damage * Mathf.Pow(1.5f, petNumber - 1)); // 이전 펫보다 1.5배 높은 능력치 부여
        }
        else
        {
            Debug.LogError("Invalid pet number. Pet number must be between 1 and 15.");
            return;
        }

        attackSpeed = petData.attackSpeed; // 플레이어의 공격 속도 가져오기
    }

    private void Start()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            // 플레이어의 scanner에서 nearestTarget을 가져옴
            if (scanner.nearestTarget != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile_uk>().target = scanner.nearestTarget;
                projectile.GetComponent<Projectile_uk>().damage = this.damage; // 플레이어의 데미지 사용
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }
}

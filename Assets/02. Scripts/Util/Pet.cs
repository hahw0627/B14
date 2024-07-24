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
        damage = petData.damage; // 기본 설정 값 사용
        attackSpeed = petData.attackSpeed; // 기본 설정 값 사용
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
                projectile.GetComponent<Projectile>().target = scanner.nearestTarget;
                projectile.GetComponent<Projectile>().damage = this.damage; // 플레이어의 데미지 사용
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }
}

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
            yield return new WaitForSeconds(1 / attackSpeed); // 1초에 / attackSpeed 만큼 공격

            // scanner가 null이 아닌 경우에만 nearestTarget을 가져와 target으로 설정
            if (scanner != null)
            {
                target = scanner.nearestTarget;
            }
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile_uk>().target = this.target;   // 생성된 투사체에 타겟 설정
            projectile.GetComponent<Projectile_uk>().damage = this.damage;   // 생성된 투사체에 데미지 설정
            projectile.GetComponent<Projectile_uk>().player = this; // 생성된 투사체에 플레이어 설정
        }
    }
}

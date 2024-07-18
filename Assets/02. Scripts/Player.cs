using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerDataSO playerData;
    public int damage;
    public float attackSpeed;
    private Scanner scanner;
    public GameObject projectilePrefab;
    private Animator animator;

    private void Awake()
    {
        damage = playerData.Damage;
        attackSpeed = playerData.AttackSpeed;
        scanner = GetComponent<Scanner>();
        animator = GetComponent<Animator>();
        Debug.Log("Damage: " + damage);
        Debug.Log("Attack Speed: " + attackSpeed);
    }

    private void Start()
    {
        StartCoroutine(Attack());
    }

    private void Update()
    {
        if (scanner.nearestTarget != null)
        {
            // target이 존재하면 IsBattel을 true로 설정
            animator.SetBool("IsBattle", true);
            Debug.Log("배틀 시작");
        }
        else
        {
            // target이 null이면 IsBattel을 false로 설정
            animator.SetBool("IsBattle", false);
            Debug.Log("배틀 종료");
        }
    }


    private IEnumerator Attack()
    {
        while (true)
        {
            // scanner의 nearestTarget이 null이 아닌 경우에만 nearestTarget을 가져와 target으로 설정 + 투사체 생성
            if (scanner.nearestTarget != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile_uk>().target = scanner.nearestTarget;   // 생성된 투사체에 타겟 설정
                projectile.GetComponent<Projectile_uk>().damage = this.damage;   // 생성된 투사체에 데미지 설정
                projectile.GetComponent<Projectile_uk>().player = this; // 생성된 투사체에 플레이어 설정
            }

            yield return new WaitForSeconds(1 / attackSpeed); // 1초에 / attackSpeed 만큼 공격
        }
    }
}

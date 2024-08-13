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
        damage = companionData.damage; // 기본 설정 값 사용
        attackSpeed = companionData.attackSpeed; // 기본 설정 값 사용
    }

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        scanner = player.GetComponent<Scanner>();
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            // 플레이어의 scanner에서 nearestTarget을 가져옴
            if (scanner.nearestTarget != null)
            {
                GameObject projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = fireMuzzle.position;
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.target = scanner.nearestTarget.transform;
                projectileScript.SetDirection(scanner.nearestTarget.transform.position);
                projectileScript.damage = this.damage; // 플레이어의 데미지 사용
                projectileScript.shooterTag = "Player";
                projectileScript.SetColor(Color.yellow);
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }
}

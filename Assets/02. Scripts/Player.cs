using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerDataSO playerData;
    public GameManager gameManager;
    public Scanner scanner;

    private Coroutine attackCoroutine;
    public Transform fireMuzzle;

    public float damage;
    public float attackSpeed;
    public int currentHp;
    private bool isUsingSkill = false;
    public int CurrentDamage { get; private set; }

    public float criticalPer;
    public float criticalMultiplier;

    private void Awake()
    {

        playerData = DataManager.Instance.playerDataSO;
        scanner = GetComponent<Scanner>();

        attackSpeed = playerData.AttackSpeed;
        currentHp = playerData.Hp;

        criticalPer = playerData.CriticalPer;
        criticalMultiplier = playerData.CriticalMultiplier;

        UpdateDamage();
    }

    private void Start()
    {
        StartAttacking();
        StartCoroutine(RecoverHp());
    }

    // 공격기능
    private IEnumerator Attack()
    {
        while (true)
        {
            // scanner의 nearestTarget이 null이 아닌 경우에만 nearestTarget을 가져와 target으로 설정 + 투사체 생성
            if (!isUsingSkill && scanner.nearestTarget != null)
            {
                GameObject projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = fireMuzzle.position;
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.target = scanner.nearestTarget;   // 생성된 투사체에 타겟 설정
                projectileScript.SetDirection(scanner.nearestTarget.transform.position);

                bool isCritical = IsCriticalHit();
                damage = CurrentDamage;

                if (isCritical)
                {
                    damage *= criticalMultiplier;
                }

                projectileScript.damage = Mathf.RoundToInt(damage);    // 생성된 투사체에 데미지 설정
                projectileScript.shooterTag = "Player";
                projectileScript.SetColor(Color.blue);
            }

            yield return new WaitForSeconds(1 / attackSpeed); // 1초에 / attackSpeed 만큼 공격
        }
    }

    private bool IsCriticalHit()
    {
        // 랜덤 값 생성 (0.0에서 100.0 사이)
        float randomValue = Random.Range(0f, 100f);
        // 랜덤 값이 치명타 확률보다 작으면 치명타 발생
        return randomValue < criticalPer;
    }

    // 체력 회복 기능
    private IEnumerator RecoverHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (currentHp < playerData.Hp)
            {
                currentHp += playerData.HpRecovery;
                if (currentHp > playerData.Hp)
                {
                    currentHp = playerData.Hp;
                }
            }
        }
    }

    // 피격 기능
    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            // 몬스터 비활성화
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                monster.SetActive(false);
            }

            StageReset();
        }
    }

    public void StageReset()
    {
        // 스테이지 페이즈 초기화
        gameManager.stagePage = 0;

        // 체력 초기화
        currentHp = playerData.Hp;
    }

    public void SetUsingSkill(bool usingSkill)
    {
        isUsingSkill = usingSkill;
    }

    public void ApplyAttackBuff(int amount)
    {
        Debug.Log($"Applying attack buff : {amount}");
        playerData.Damage += amount;
        UpdateDamage();
    }
    private void UpdateDamage()
    {
        Debug.Log($"Updating damage. Base: {playerData.Damage}, Buff: {playerData.Damage}");
        CurrentDamage = playerData.Damage;
        damage = CurrentDamage;
        Debug.Log($"New damage: {CurrentDamage}");
    }

    public void Heal(int amount) // 추후 플레이어 피격 구현시 구현 예정
    {

    }





    public void StartAttacking()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(Attack());
        }
    }

    public void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
}

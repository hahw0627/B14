using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerDataSO playerData;
    public int damage;
    public float attackSpeed;
    public int currentHp;
    public GameObject projectilePrefab;
    public MonsterSpawner_UK monsterSpawner;
    public Scanner scanner;
    private Animator animator;
    public SpriteRenderer spriteRenderer;
    private bool isUsingSkill = false;
    private Coroutine attackCoroutine;
    public int CurrentDamage { get; private set; }

    private void Awake()
    {
        UpdateDamage();

        damage = playerData.Damage;
        attackSpeed = playerData.AttackSpeed;
        currentHp = playerData.Hp;

        scanner = GetComponent<Scanner>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartAttacking();
        StartCoroutine(RecoverHp());
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
            if (!isUsingSkill && scanner.nearestTarget != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile_uk>().target = scanner.nearestTarget;   // 생성된 투사체에 타겟 설정
                projectile.GetComponent<Projectile_uk>().damage = this.damage;   // 생성된 투사체에 데미지 설정
                projectile.GetComponent<Projectile_uk>().player = this; // 생성된 투사체에 플레이어 설정
            }

            yield return new WaitForSeconds(1 / attackSpeed); // 1초에 / attackSpeed 만큼 공격
        }
    }

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
        Debug.Log($"New damage: {CurrentDamage}");
    }

    public void Heal(int amount) // 추후 플레이어 피격 구현시 구현 예정
    {

    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Player HP 감소! : {currentHp} - {damage}");


        if (currentHp <= 0)
        {
            if (monsterSpawner != null)
            {
                monsterSpawner.stagePage = 0;
            }
            else
            {
                Debug.LogError("MonsterSpawner_UK 연결 실패.");
            }

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                monster.SetActive(false);
            }

            Debug.Log("플레이어 사망. 몬스터 비활성화. 스테이지 초기화");

            StartCoroutine(PlayerDeath()); // 체력 0 이하일 때 투명도 조절 코루틴 실행
        }
    }

    private IEnumerator PlayerDeath()
    {
        // 1초 동안 2번 255에서 70까지 감소하고 다시 255까지 증가
        for (int i = 0; i < 2; i++)
        {
            // 255에서 70까지 감소
            for (float alpha = 1.0f; alpha >= 0.275f; alpha -= Time.deltaTime * 2)
            {
                SetSpriteAlpha(alpha);
                yield return null;
            }
            // 70에서 255까지 증가
            for (float alpha = 0.275f; alpha <= 1.0f; alpha += Time.deltaTime * 2)
            {
                SetSpriteAlpha(alpha);
                yield return null;
            }
        }

        // 체력 및 스테이지 초기화
        currentHp = playerData.Hp;
        Debug.Log("플레이어 체력 복구");
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}

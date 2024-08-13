using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class Monster : MonoBehaviour, IDamageable
{
    public MonsterDataSO monsterData;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject target;

    public int Hp;
    public int damage;
    public float attackSpeed;
    public float moveTime = 0.0f;

    public bool isAttacking = false;
    public Transform hudPos;
    protected DamageTextPool damageTextPool;

    private int goldReward;

    public event Action<Monster> OnDeath;

    private void Awake()
    {
        goldReward = 10;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        damageTextPool = FindObjectOfType<DamageTextPool>();
        if (damageTextPool == null)
        {
            Debug.LogError("DamageTextPool not found in the scene. Make sure it exists.");
        }
    }

    // 몬스터 활성화 시
    protected virtual void OnEnable()
    {
        if (monsterData == null)
        {
            Debug.LogError("MonsterDataSO_Test instance fale");
            return;
        }

        Hp = monsterData.Hp;
        damage = monsterData.Damage;
        attackSpeed = monsterData.AttackSpeed;
        moveTime = 0.0f;
        isAttacking = false;
    }

    void Update()
    {
        MoveAndAttck();
    }

    // 이동과 공격 실행
    private void MoveAndAttck()
    {
        if (moveTime < 1.5f)
        {
            spriteRenderer.flipX = true;
            transform.Translate(Vector3.left * 2.0f * Time.deltaTime);
            moveTime += Time.deltaTime;
        }
        else
        {
            animator.SetBool("IsBattle", true);
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
    }

    // 몬스터 공격
    private IEnumerator Attack()
    {
        isAttacking = true;
        while (true)
        {
            if (target != null)
            {
                GameObject projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = transform.position;
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.target = target.transform;
                projectileScript.SetDirection(target.transform.position);
                projectileScript.damage = damage;
                projectileScript.shooterTag = "Monster";
                projectileScript.SetColor(Color.red);
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }

    // 몬스터 피격
    public virtual void TakeDamage(int damage, bool isSkillDamage = false)
    {
        if (damageTextPool != null)
        {
            DamageText damageText = damageTextPool.GetDamageText();
            if (damageText != null)
            {
                damageText.transform.position = hudPos.position;
                damageText.SetDamage(damage);
            }
            else
            {
                Debug.LogWarning("Failed to get DamageText from pool.");
            }
        }
        else
        {
            Debug.LogWarning("DamageTextPool is not initialized.");
        }

        Hp -= damage;
        if (Hp <= 0)
        {
            Die();
            QuestTest.Instance.CountOneQuestSuccess();
            gameObject.SetActive(false);
        }
    }

    // 몬스터 사망
    public void Die()
    {
        var instance = DataManager.Instance;
        instance.playerDataSO.Gold += goldReward;
        OnDeath?.Invoke(this);
    }
}

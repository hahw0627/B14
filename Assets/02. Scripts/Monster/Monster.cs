using UnityEngine;
using System.Collections;
using System;
using _10._Externals.HeroEditor4D.Common.Scripts.CharacterScripts;
using UnityEngine.Serialization;

public class Monster : MonoBehaviour, IDamageable
{
    [FormerlySerializedAs("monsterData")]
    public MonsterDataSO MonsterData;

    [FormerlySerializedAs("target")]
    public GameObject Target;

    public int Hp;

    [FormerlySerializedAs("damage")]
    public int Damage;

    [FormerlySerializedAs("attackSpeed")]
    public float AttackSpeed;

    [FormerlySerializedAs("moveTime")]
    public float MoveTime;

    [FormerlySerializedAs("isAttacking")]
    public bool IsAttacking;

    [FormerlySerializedAs("hudPos")]
    public Transform HUDPos;

    [FormerlySerializedAs("fireMuzzle")]
    public Transform FireMuzzle;

    protected DamageTextPool DamageTextPool;
    public Animator animator;

    private int _goldReward;

    public event Action<Monster> onDeath;
    
    private void Awake()
    {
        _goldReward = 10;
        //_animator = GetComponent<Animator>();
        Target = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        DamageTextPool = FindObjectOfType<DamageTextPool>();
        if (DamageTextPool == null)
        {
            Debug.LogError("<color=red>DamageTextPool not found in the scene. Make sure it exists.</color>");
        }
    }

    // 몬스터 활성화 시
    protected virtual void OnEnable()
    {
        if (MonsterData == null)
        {
            Debug.LogError("<color=red>MonsterDataSO_Test instance is missing</color>");
            return;
        }

        Hp = MonsterData.Hp;
        Damage = MonsterData.Damage;
        AttackSpeed = MonsterData.AttackSpeed;
        MoveTime = 0.0f;
        IsAttacking = false;
    }

    private void Update()
    {
        #region MoveAndAttack

        if (MoveTime < 1.5f)
        {
            transform.Translate(Vector3.left * (2.0f * Time.deltaTime));
            MoveTime += Time.deltaTime;
        }
        else
        {
            //_animator.SetBool(IsBattle, true);
            if (!IsAttacking)
            {
                StartCoroutine(Attack());
            }
        }

        #endregion
    }

    // 몬스터 공격
    private IEnumerator Attack()
    {
        IsAttacking = true;
        while (true)
        {
            if (Target is not null)
            {
                animator.SetTrigger("Slash1H");
                var projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = FireMuzzle.position;
                var projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.Target = Target.transform;
                projectileScript.SetDirection(Target.transform.position);
                projectileScript.Damage = Damage;
                projectileScript.ShooterTag = "Monster";
                projectileScript.SetColor(Color.red);
            }

            yield return new WaitForSeconds(1 / AttackSpeed);
        }
    }

    // 몬스터 피격
    public virtual void TakeDamage(int damage, bool isSkillDamage = false, bool isPetAttack = false)
    {
        if (DamageTextPool is not null)
        {
            var damageText = DamageTextPool.GetDamageText();
            if (damageText is not null)
            {
                damageText.transform.position = HUDPos.position;
                bool isCritical = UnityEngine.Random.Range(0f, 100f) < DataManager.Instance.PlayerDataSo.CriticalPer;

                if (isPetAttack)
                {
                    damageText.SetDamage(damage, false, Color.yellow, 0.8f);
                }
                else if (isCritical)
                {
                    damage = Mathf.RoundToInt(damage * DataManager.Instance.PlayerDataSo.CriticalMultiplier);
                    damageText.SetDamage(damage, true);
                }
                else
                {
                    damageText.SetDamage(damage);
                }
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
        if (Hp > 0) return;
        Die();
        QuestTest.Instance.CountOneQuestSuccess();
        gameObject.SetActive(false);
    }

    // 몬스터 사망
    public void Die()
    {
        var instance = DataManager.Instance;
        instance.PlayerDataSo.Gold += _goldReward;
        onDeath?.Invoke(this);
    }
}

using UnityEngine;
using System.Collections;
using System;
using _10._Externals.HeroEditor4D.Common.Scripts.CharacterScripts;
using UnityEngine.Serialization;

public class Monster : MonoBehaviour, IDamageable
{
    public Character4D Character;
    
    [FormerlySerializedAs("monsterData")]
    public MonsterDataSO MonsterData;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
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
    protected DamageTextPool DamageTextPool;

    private int _goldReward;
    private static readonly int IsBattle = Animator.StringToHash("IsBattle");

    public event Action<Monster> onDeath;

    private void Awake()
    {
        _goldReward = 10;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Target = GameObject.Find("Player");
        DamageTextPool = FindObjectOfType<DamageTextPool>();
        if (DamageTextPool == null)
        {
            Debug.LogError("DamageTextPool not found in the scene. Make sure it exists.");
        }
    }

    // 몬스터 활성화 시
    protected virtual void OnEnable()
    {
        if (MonsterData == null)
        {
            Debug.LogError("MonsterDataSO_Test instance is missing");
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
            _spriteRenderer.flipX = true;
            transform.Translate(Vector3.left * (2.0f * Time.deltaTime));
            MoveTime += Time.deltaTime;
        }
        else
        {
            _animator.SetBool(IsBattle, true);
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
                //Character.AnimationManager.Fire();
                var projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = transform.position;
                var projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.target = Target.transform;
                projectileScript.SetDirection(Target.transform.position);
                projectileScript.damage = Damage;
                projectileScript.shooterTag = "Monster";
                projectileScript.SetColor(Color.red);
            }
            yield return new WaitForSeconds(1 / AttackSpeed);
        }
    }

    // 몬스터 피격
    public virtual void TakeDamage(int damage, bool isSkillDamage = false)
    {
        if (DamageTextPool is not null)
        {
            var damageText = DamageTextPool.GetDamageText();
            if (damageText is not null)
            {
                damageText.transform.position = HUDPos.position;
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
        if (Hp > 0) return;
        Die();
        QuestTest.Instance.CountOneQuestSuccess();
        gameObject.SetActive(false);
    }

    // 몬스터 사망
    public void Die()
    {
        var instance = DataManager.Instance;
        instance.playerDataSO.Gold += _goldReward;
        onDeath?.Invoke(this);
    }
}

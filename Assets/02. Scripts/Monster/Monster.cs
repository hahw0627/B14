using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Serialization;
using _10._Externals.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;

public class Monster : MonoBehaviour, IDamageable
{
    [FormerlySerializedAs("monsterData")]
    public MonsterDataSO MonsterData;

    [SerializeField]
    private HpBar _hpBar;
    
    [FormerlySerializedAs("target")]
    public GameObject Target;

    public long CurrentHp;

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
    private Animator _animator;
    private AnimationManager _animationManager;

    private long _goldReward;
    private static readonly int Slash1H = Animator.StringToHash("Slash1H");

    public event Action<Monster> onDeath;
    
    private void Awake()
    {
        _goldReward = 100;
        Target = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        _animationManager = GetComponent<AnimationManager>();
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

        CurrentHp = MonsterData.MaxHp;
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
            _animationManager.SetState(CharacterState.Walk);
            transform.Translate(Vector3.left * (2.0f * Time.deltaTime));
            MoveTime += Time.deltaTime;
        }
        else
        {
            _animationManager.SetState(CharacterState.Idle);
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
                if(name == "Goblin_Archer(Clone)" || name == "Goblin_Archer_Boss")
                {
                    _animator.SetTrigger("ShotBow");
                }
                else
                {
                    _animator.SetTrigger(Slash1H);
                }
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

        CurrentHp -= damage;
        if (CurrentHp > 0) return;
        Die();
        QuestTest.Instance.CountOneQuestSuccess();
        gameObject.SetActive(false);
    }

    // 몬스터 사망
    public void Die()
    {
        //_animator.SetTrigger("Idle"); 죽었을 때, 공격 애니메이션 멈추기
        var instance = DataManager.Instance;
        instance.PlayerDataSo.Gold += _goldReward * StageManager.Instance.StageDataSO.Stage;
        onDeath?.Invoke(this);
    }
}

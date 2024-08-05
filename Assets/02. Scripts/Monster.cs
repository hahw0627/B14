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

    private bool isAttacking = false;
    public GameObject hudDamgeText;
    public Transform hudPos;

    private int goldReward;

    public event Action<Monster> OnDeath;

    private void Awake()
    {
        goldReward = 10;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
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
        GameObject hudText = Instantiate(hudDamgeText);
        hudText.transform.position = hudPos.position;

        DamageText damageTextComponent = hudText.GetComponent<DamageText>();

        if(damageTextComponent != null)
        {
            damageTextComponent.SetDamage(damage);
        }

        Hp -= damage;
        if (Hp <= 0)
        {
            this.Die();
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








//==================================================================================================
//using System.Collections;
//using UnityEngine;
//using Vector3 = UnityEngine.Vector3;

//public class Monster : MonoBehaviour
//{
//    [SerializeField]
//    private MonsterStatistics _monsterStatistics;
//    private int Hp;

//    public MonsterStatistics MonsterStatistics
//    {
//        set => _monsterStatistics = value;
//    }

//    [SerializeField]
//    private float _moveSpeed = 2.0f;

//    private bool _isCollision;

//    private void Start()
//    {
//        Debug.Log($"{_monsterStatistics.Name} ���� �Ϸ�");
//        Hp = _monsterStatistics.Hp;
//        StartCoroutine(MoveForSeconds(1.5f));
//    }

//    private void Update()
//    {
//        if (_isCollision) return;
//    }

//    private IEnumerator MoveForSeconds(float duration)
//    {
//        float moveTime = 0.0f;

//        while (moveTime < duration)
//        {
//            transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
//            moveTime += Time.deltaTime;
//            yield return null;
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        _isCollision = true;
//        if (other.gameObject.name != "Player(Test)") return;
//        Debug.Log("---");
//        Debug.Log($"{_monsterStatistics.Name} ���� ����");
//        StartCoroutine(nameof(Attack));
//    }

//    private IEnumerator Attack()
//    {
//        Debug.Log($"�÷��̾� ü��: {PlayerTest.CurrentHp} / {PlayerTest.MaxHp}");
//        while (true)
//        {
//            if (PlayerTest.CurrentHp <= 0) yield break;
//            yield return new WaitForSeconds(_monsterStatistics.AttackDelay);
//            PlayerTest.CurrentHp -= _monsterStatistics.Attack;
//            Debug.Log($"�÷��̾� ü��: {PlayerTest.CurrentHp} / {PlayerTest.MaxHp}");
//        }
//    }

//    public void TakeDamage(int damage)
//    {
//        Hp -= damage;
//        Debug.Log("���� HP ����\n" + "HP : " + Hp + " / ������ : " + damage);

//        if (Hp <= 0)
//        {
//            MonsterPool.InsertQueue(gameObject);
//        }
//    }
//}
using UnityEngine;
using System.Collections;
using TMPro;

public class Monster123 : MonoBehaviour, IDamageable
{
    public MonsterDataSO monsterData;
    public PlayerDataSO playerData;
    public GameObject monsterProjectilePrefab;
    public Transform target;
    public int Hp;
    public int damage;
    public float attackSpeed;
    private float moveTime = 0.0f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (monsterData == null)
        {
            Debug.LogError("MonsterDataSO_Test 연결 실패");
            return;
        }
        // 활성화될 때 몬스터의 데이터를 초기화
        Hp = monsterData.Hp;
        damage = monsterData.Damage;
        attackSpeed = monsterData.AttackSpeed;
        moveTime = 0.0f; // moveTime 초기화
        isAttacking = false;
    }

    private void Update()
    {
        // 몬스터 이동 시작
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

    private IEnumerator Attack()
    {
        isAttacking = true;
        while (true)
        {
            if (target != null)
            {
                GameObject projectile = Instantiate(monsterProjectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<MonsterProjectile>().target = target;
                projectile.GetComponent<MonsterProjectile>().damage = damage;
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("몬스터 HP 감소\n" + "HP : " + Hp + " / 데미지 : " + damage);

        if (Hp <= 0)
        {
            gameObject.SetActive(false);
            // UI에 연결하여 증가 확인할 수 있게 해줄 것
            playerData.Gold += 10;
            Debug.Log("비활성화");
        }
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
//        Debug.Log($"{_monsterStatistics.Name} 생성 완료");
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
//        Debug.Log($"{_monsterStatistics.Name} 공격 시작");
//        StartCoroutine(nameof(Attack));
//    }

//    private IEnumerator Attack()
//    {
//        Debug.Log($"플레이어 체력: {PlayerTest.CurrentHp} / {PlayerTest.MaxHp}");
//        while (true)
//        {
//            if (PlayerTest.CurrentHp <= 0) yield break;
//            yield return new WaitForSeconds(_monsterStatistics.AttackDelay);
//            PlayerTest.CurrentHp -= _monsterStatistics.Attack;
//            Debug.Log($"플레이어 체력: {PlayerTest.CurrentHp} / {PlayerTest.MaxHp}");
//        }
//    }

//    public void TakeDamage(int damage)
//    {
//        Hp -= damage;
//        Debug.Log("몬스터 HP 감소\n" + "HP : " + Hp + " / 데미지 : " + damage);

//        if (Hp <= 0)
//        {
//            MonsterPool.InsertQueue(gameObject);
//        }
//    }
//}
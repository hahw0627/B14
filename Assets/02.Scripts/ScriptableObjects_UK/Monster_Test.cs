using UnityEngine;
using System.Collections;

public class Monster_Test : MonoBehaviour
{
    public MonsterDataSO_Test monsterData;
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
                projectile.GetComponent<MonsterProjectile_uk>().target = target;
                projectile.GetComponent<MonsterProjectile_uk>().damage = damage;
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
            Debug.Log("비활성화");
        }
    }
}

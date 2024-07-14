using UnityEngine;
using System.Collections;

public class Monster_Test : MonoBehaviour
{
    public MonsterDataSO_Test monsterData;
    public int Hp;
    public int damage;
    public float attackSpeed;
    private float moveTime = 0.0f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Ȱ��ȭ�� �� ������ �����͸� �ʱ�ȭ
        Hp = monsterData.Hp;
        damage = monsterData.Damage;
        attackSpeed = monsterData.AttackSpeed;
        moveTime = 0.0f; // moveTime �ʱ�ȭ
    }

    private void Update()
    {
        // ���� �̵� ����
        if (moveTime < 1.5f)
        {
            spriteRenderer.flipX = true;
            transform.Translate(Vector3.left * 2.0f * Time.deltaTime);
            moveTime += Time.deltaTime;
        }
        else if (moveTime >= 1.5f)
        {
            animator.SetBool("IsBattle", true);
        }
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("���� HP ����\n" + "HP : " + Hp + " / ������ : " + damage);

        if (Hp <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("��Ȱ��ȭ");
        }
    }
}

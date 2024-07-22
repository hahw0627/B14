using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private MonsterStatistics _monsterStatistics;
    private int Hp;

    public MonsterStatistics MonsterStatistics
    {
        set => _monsterStatistics = value;
    }

    [SerializeField]
    private float _moveSpeed = 2.0f;

    private bool _isCollision;

    private void Start()
    {
        Debug.Log($"{_monsterStatistics.Name} 생성 완료");
        Hp = _monsterStatistics.Hp;
        StartCoroutine(MoveForSeconds(1.5f));
    }

    private void Update()
    {
        if (_isCollision) return;
    }

    private IEnumerator MoveForSeconds(float duration)
    {
        float moveTime = 0.0f;

        while (moveTime < duration)
        {
            transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _isCollision = true;
        if (other.gameObject.name != "Player(Test)") return;
        Debug.Log("---");
        Debug.Log($"{_monsterStatistics.Name} 공격 시작");
        StartCoroutine(nameof(Attack));
    }

    private IEnumerator Attack()
    {
        Debug.Log($"플레이어 체력: {PlayerTest.CurrentHp} / {PlayerTest.MaxHp}");
        while (true)
        {
            if (PlayerTest.CurrentHp <= 0) yield break;
            yield return new WaitForSeconds(_monsterStatistics.AttackDelay);
            PlayerTest.CurrentHp -= _monsterStatistics.Attack;
            Debug.Log($"플레이어 체력: {PlayerTest.CurrentHp} / {PlayerTest.MaxHp}");
        }
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("몬스터 HP 감소\n" + "HP : " + Hp + " / 데미지 : " + damage);

        if (Hp <= 0)
        {
            MonsterPool.InsertQueue(gameObject);
        }
    }
}
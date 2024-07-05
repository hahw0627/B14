using UnityEngine;
using System.Collections;

public class Monster_Test : MonoBehaviour
{
    public MonsterDataSO_Test monsterData;
    private int Hp;

    private void Start()
    {
        Hp = monsterData.Hp;
        StartCoroutine(MoveForSeconds(1.5f));
    }

    private IEnumerator MoveForSeconds(float duration)
    {
        float moveSpeed = 2.0f;
        float moveTime = 0.0f;

        while (moveTime < duration)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("몬스터 HP 감소\n" + "HP : " + Hp + " / 데미지 : " + damage);

        if (Hp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("삭제");
        }
    }
}

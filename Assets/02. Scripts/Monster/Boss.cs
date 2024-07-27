using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Boss : Monster123  // ���� ��ũ��Ʈ ���
{
    public MonsterSpawner monsterSpawner;

    // ���� ���� Ȱ��ȭ ��
    protected override void OnEnable()
    {
        Hp = monsterData.Hp * 3; // ���� ���ʹ� HP�� 2��� ����
        damage = monsterData.Damage * 2; // ���� ���ʹ� �������� 2��� ����
        attackSpeed = monsterData.AttackSpeed;
    }

    // ���� ���� �ǰ�
    public override void TakeDamage(int damage)
    {
        GameObject hudText = Instantiate(hudDamgeText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().SetDamage(DataManager.Instance.playerDataSO.Damage);
        Hp -= damage;

        if (Hp <= 0)
        {
            this.Die();
            BossDeath();
            gameObject.SetActive(false);
        }
    }

    // ���� ���
    public void BossDeath()
    {
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� StagePage�� 0�� �ȴ�.
        monsterSpawner.stagePage = 0;
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� Stage�� 1 ������Ų��.
        monsterSpawner.stage++;
        monsterData.stage = monsterSpawner.stage;  // ����SO�� �������� ���� ����?
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� MonsterDataSO_Test�� ���� 1.2f ���ϰ� ��Ʈ������ ��ȯ�ؼ� ����
        monsterData.Hp = Mathf.RoundToInt(monsterData.Hp * 1.2f);
        monsterData.Damage = Mathf.RoundToInt(monsterData.Damage * 1.2f);
    }
}

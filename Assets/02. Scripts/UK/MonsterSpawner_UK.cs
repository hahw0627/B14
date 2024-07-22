using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner_UK : MonoBehaviour
{
    public MonsterDataSO_Test monsterData;
    public GameObject monsterPrefab;
    public GameObject bossMonster;
    public Transform[] spawnPoints;
    public GameObject[] monsters;
    public int stagePage = 0;
    public int stage = 1;
    public Transform target;

    private void Start()
    {
        if (spawnPoints.Length < 6)
        {
            Debug.LogError("�������� ���� ����.");
            return;
        }

        bossMonster.SetActive(false);
        // ������ �����ϸ� ���� 6���� �����Ͽ� �迭�� ��Ȱ��ȭ ���·� ����
        monsters = new GameObject[6];
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i] = Instantiate(monsterPrefab);
            monsters[i].SetActive(false);
        }

        StartCoroutine(CheckMonsters());
    }

    private IEnumerator CheckMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // ���� �迭�� ���� ��Ȱ��ȭ �Ǿ��ִ��� Ȯ��
            if (AllMonstersDeactivated())
            {
                // StagePage�� 1 ����
                stagePage++;
                if (stagePage <= 3)
                {
                    SpawnMonsters();
                }
                else if (stagePage == 4)
                {
                    SpawnBoss();
                }
            }
        }
    }

    private bool AllMonstersDeactivated()
    {
        foreach (GameObject monster in monsters)
        {
            if (monster.activeSelf) return false;
        }
        return true;
    }

    private void SpawnMonsters()
    {
        // StagePage�� 1 �����ϸ� ������ �迭�� ��ġ���� ���͸� Ȱ��ȭ
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].transform.position = spawnPoints[i].position;
            monsters[i].GetComponent<Monster_Test>().monsterData = monsterData;
            monsters[i].GetComponent<Monster_Test>().target = target;
            monsters[i].SetActive(true);
        }
    }

    private void SpawnBoss()
    {
        bossMonster.transform.position = spawnPoints[3].position;
        bossMonster.GetComponent<Boss>().monsterData = monsterData;
        bossMonster.SetActive(true);
    }

    public void BossDeath()
    {
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� StagePage�� 0�� �ȴ�.
        stagePage = 0;
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� Stage�� 1 ������Ų��.
        stage++;
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� MonsterDataSO_Test�� ���� 1.2f ���ϰ� ��Ʈ������ ��ȯ�ؼ� ����
        monsterData.Hp = Mathf.RoundToInt(monsterData.Hp * 1.2f);
        monsterData.Damage = Mathf.RoundToInt(monsterData.Damage * 1.2f);
    }
}

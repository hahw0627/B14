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
            Debug.LogError("스폰지점 연결 실패.");
            return;
        }

        bossMonster.SetActive(false);
        // 게임이 시작하면 몬스터 6마리 생성하여 배열에 비활성화 상태로 저장
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

            // 몬스터 배열이 전부 비활성화 되어있는지 확인
            if (AllMonstersDeactivated())
            {
                // StagePage를 1 증가
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
        // StagePage가 1 증가하면 스포너 배열의 위치에서 몬스터를 활성화
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
        // BossMonster의 HP가 0 이하가 되면 StagePage는 0이 된다.
        stagePage = 0;
        // BossMonster의 HP가 0 이하가 되면 Stage를 1 증가시킨다.
        stage++;
        // BossMonster의 HP가 0 이하가 되면 MonsterDataSO_Test의 값을 1.2f 곱하고 인트형으로 변환해서 저장
        monsterData.Hp = Mathf.RoundToInt(monsterData.Hp * 1.2f);
        monsterData.Damage = Mathf.RoundToInt(monsterData.Damage * 1.2f);
    }
}

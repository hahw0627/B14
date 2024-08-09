using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MonsterSpawner spawner;

    public int stagePage = 0;
    public int stage = 1;

    private void Start()
    {
        StartCoroutine(CheckMonsters());
    }

    // 몬스터 상태 확인 + 소환
    private IEnumerator CheckMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // 몬스터 배열이 전부 비활성화 되어있는지 확인
            if (spawner.AllMonstersDeactivated())
            {
                // StagePage를 1 증가
                stagePage++;
                if (stagePage <= 3)
                {
                    spawner.SpawnMonsters();
                }
                else if (stagePage == 4)
                {
                    spawner.SpawnBoss();
                }
            }
        }
    }
}

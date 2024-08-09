using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject bossMonster;

    public GameManager gameManager;

    // 모든 몬스터 비활성화 확인
    public bool AllMonstersDeactivated()
    {
        foreach (GameObject monster in MonsterPool.Instance.monsters)
        {
            if (monster.activeSelf) return false;
        }
        return true;
    }

    // 몬스터 소환
    public void SpawnMonsters()
    {
        // StagePage가 1 증가하면 스포너 배열의 위치에서 몬스터를 활성화
        if(gameManager.stage <= 2)
        {
            ActiveMonsters(1);
        }
        else if(gameManager.stage >= 3 && gameManager.stage <= 5)
        {
            ActiveMonsters(3);
        }
        else
        {
            ActiveMonsters(6);
        }
    }

    // 보스 소환
    public void SpawnBoss()
    {
        bossMonster.transform.position = spawnPoints[3].position;
        bossMonster.SetActive(true);
    }

    private void ActiveMonsters(int num)
    {
        for (int i = 0; i < num; i++)
        {
            MonsterPool.Instance.monsters[i].transform.position = spawnPoints[i].position;
            MonsterPool.Instance.monsters[i].SetActive(true);
        }
    }
}


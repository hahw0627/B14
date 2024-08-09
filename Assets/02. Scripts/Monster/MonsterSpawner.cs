using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject bossMonster;

    public GameManager gameManager;

    // ��� ���� ��Ȱ��ȭ Ȯ��
    public bool AllMonstersDeactivated()
    {
        foreach (GameObject monster in MonsterPool.Instance.monsters)
        {
            if (monster.activeSelf) return false;
        }
        return true;
    }

    // ���� ��ȯ
    public void SpawnMonsters()
    {
        // StagePage�� 1 �����ϸ� ������ �迭�� ��ġ���� ���͸� Ȱ��ȭ
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

    // ���� ��ȯ
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


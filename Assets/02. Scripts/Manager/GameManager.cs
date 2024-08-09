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

    // ���� ���� Ȯ�� + ��ȯ
    private IEnumerator CheckMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // ���� �迭�� ���� ��Ȱ��ȭ �Ǿ��ִ��� Ȯ��
            if (spawner.AllMonstersDeactivated())
            {
                // StagePage�� 1 ����
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

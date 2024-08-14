using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterSpawner : MonoBehaviour
{
    [FormerlySerializedAs("spawnPoints")]
    public Transform[] SpawnPoints;

    [FormerlySerializedAs("bossMonster")]
    public GameObject BossMonster;

    [FormerlySerializedAs("gameManager")]
    public GameManager GameManager;

    // ��� ���� ��Ȱ��ȭ Ȯ��
    public bool AllMonstersDeactivated()
    {
        if (MonsterPool.Instance.Monsters.Any(monster => monster.activeSelf))
        {
            return false;
        }

        return !BossMonster.activeSelf;
    }

    // ���� ��ȯ
    public void SpawnMonsters()
    {
        switch (StageManager.Instance.StageDataSO.Stage)
        {
            // StagePage�� 1 �����ϸ� ������ �迭�� ��ġ���� ���͸� Ȱ��ȭ
            case <= 2:
                ActiveMonsters(1);
                break;
            case >= 3 and <= 5:
                ActiveMonsters(3);
                break;
            default:
                ActiveMonsters(6);
                break;
        }
    }

    // ���� ��ȯ
    public void SpawnBoss()
    {
        BossMonster.transform.position = SpawnPoints[3].position;
        BossMonster.SetActive(true);
    }

    private void ActiveMonsters(int num)
    {
        for (var i = 0; i < num; i++)
        {
            MonsterPool.Instance.Monsters[i].transform.position = SpawnPoints[i].position;
            MonsterPool.Instance.Monsters[i].SetActive(true);
        }
    }

    public IEnumerator CheckMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (AllMonstersDeactivated())
            {
                if (StageManager.Instance.StageDataSO.StagePage < 4)
                {
                    StageManager.Instance.ChangeStage(StageManager.Instance.StageDataSO.Stage,
                        ++StageManager.Instance.StageDataSO.StagePage);
                }

                switch (StageManager.Instance.StageDataSO.StagePage)
                {
                    case <= 3:
                        SpawnMonsters();
                        break;
                    default:
                        SpawnBoss();
                        break;
                }
            }
        }
    }
}
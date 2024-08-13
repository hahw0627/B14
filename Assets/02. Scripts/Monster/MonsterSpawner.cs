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
        switch (GameManager.Stage)
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
        for (int i = 0; i < num; i++)
        {
            MonsterPool.Instance.Monsters[i].transform.position = SpawnPoints[i].position;
            MonsterPool.Instance.Monsters[i].SetActive(true);
        }
    }
}

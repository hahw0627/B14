using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    
    [FormerlySerializedAs("spawnPoints")]
    public Transform[] SpawnPoints;

    [FormerlySerializedAs("bossMonster")]
    public GameObject[] BossMonsters;

    public bool AllMonstersDeactivated()
    {
        if (MonsterPool.Instance.Monsters.Any(monster => monster.activeSelf) ||
            BossMonsters.Any(boss => boss.activeSelf))
        {
            return false;
        }

        return true;
    }

    public void SpawnMonsters()
    {
        if (_player.CurrentHp <= 0 || StageManager.Instance.StageDataSO.StagePage == 0) return;
        var currentStage = StageManager.Instance.StageDataSO.Stage;

        switch (currentStage)
        {
            case <= 4:
                ActiveMonsters(1);
                break;
            case <= 10:
                ActiveMonsters(3);
                break;
            default:
                ActiveMonsters(6);
                break;
        }
    }

    private void SpawnBoss()
    {
        if (_player.CurrentHp <= 0) return;
        switch (StageManager.Instance.StageDataSO.Stage)
        {
            case <= 4:
                ActiveBoss(0);
                break;
            case <= 10:
                ActiveBoss(1);
                break;
            default:
                ActiveBoss(2);
                break;
        }
    }

    private void ActiveBoss(int num)
    {
        if (_player.CurrentHp <= 0) return;
        BossMonsters[num].transform.position = SpawnPoints[3].position;
        BossMonsters[num].GetComponent<SortingGroup>().sortingOrder = 202;
        BossMonsters[num].SetActive(true);
    }

    private void ActiveMonsters(int num)
    {
        if (_player.CurrentHp <= 0) return;
        for (var i = 0; i < num; i++)
        {
            MonsterPool.Instance.Monsters[i].transform.position = SpawnPoints[i].position;
            MonsterPool.Instance.Monsters[i].SetActive(true);
            MonsterPool.Instance.Monsters[i].GetComponent<SortingGroup>().sortingOrder = i switch
            {
                0 or 3 => 202,
                1 or 4 => 204,
                _ => 201
            };
        }
    }

    public IEnumerator CheckMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (!AllMonstersDeactivated()) continue;

            if (StageManager.Instance.StageDataSO.StagePage < 4 && _player.CurrentHp > 0)
            {
                StageManager.Instance.ChangeStage(StageManager.Instance.StageDataSO.Stage,
                    ++StageManager.Instance.StageDataSO.StagePage);
            }
            
            if (StageManager.Instance.StageDataSO.Stage == 1 && StageManager.Instance.StageDataSO.StagePage == 1)
            {
                yield return new WaitForSeconds(4f);
                SpawnMonsters();
            }
            else
            {
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
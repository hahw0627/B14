using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject bossMonster;

    public int stagePage = 0;
    public int stage = 1;

    private void Start()
    {
        if (spawnPoints.Length < 6)
        {
            Debug.LogError("스폰지점 연결 실패.");
            return;
        }

        bossMonster.SetActive(false);

        StartCoroutine(CheckMonsters());
    }

    
    // 몬스터 상태 확인 + 소환
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

    // 모든 몬스터 비활성화 확인
    private bool AllMonstersDeactivated()
    {
        foreach (GameObject monster in MonsterPool.Instance.monsters)
        {
            if (monster.activeSelf) return false;
        }
        return true;
    }

    // 몬스터 소환
    private void SpawnMonsters()
    {
        // StagePage가 1 증가하면 스포너 배열의 위치에서 몬스터를 활성화
        for (int i = 0; i < MonsterPool.Instance.monsters.Length; i++)
        {
            MonsterPool.Instance.monsters[i].transform.position = spawnPoints[i].position;
            //MonsterPool.Instance.monsters[i].GetComponent<Monster123>().target = target;
            MonsterPool.Instance.monsters[i].SetActive(true);
        }
    }

    // 보스 소환
    private void SpawnBoss()
    {
        bossMonster.transform.position = spawnPoints[3].position;
        bossMonster.SetActive(true);
    }
}

















//============================================================================================================================
//using System.Collections.Generic;
//using UnityEngine;

//namespace _02._Scripts.Monster
//{
//    public class MonsterSpawner : MonoBehaviour
//    {
//        [SerializeField]
//        private List<MonsterStatistics> _monsterStatistics;

//        [SerializeField]
//        private List<Transform> _monsterPositions;

//        [SerializeField]
//        private GameObject _monsterPrefab;

//        private GameObject _monster;

//        private void Start()
//        {
//            StartMonsterSpawn();
//        }

//        private void StartMonsterSpawn()
//        {
//            foreach (Transform t in _monsterPositions)
//            {
//                SpawnMonster((MonsterType)StageTest.StageLevel,
//                    new Vector3(t.position.x, t.position.y, 0.0f));
//            }
//        }

//        // 몬스터 비활성화 테스트를 위한 코드(합치고 나서 삭제 예정) 
//        private void Update()
//        {
//            if (!PlayerTest.isAlive)
//            {
//                Die();
//            }
//        }

//        private void SpawnMonster(MonsterType type, Vector3 position)
//        {
//            _monster = MonsterPool.Monsters.Count == 0 ? Instantiate(_monsterPrefab, position, Quaternion.identity) :
//                //MonsterPool.Monsters.Enqueue(_monster.gameObject);
//                MonsterPool.GetQueue();
//            _monster.GetComponent<global::Monster>().MonsterStatistics = _monsterStatistics[(int)type];
//        }

//        private void Die()
//        {
//            MonsterPool.InsertQueue(gameObject);
//        }
//    }
//}
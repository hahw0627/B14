using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public MonsterDataSO monsterData;
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
            monsters[i].GetComponent<Monster123>().monsterData = monsterData;
            monsters[i].GetComponent<Monster123>().target = target;
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
        monsterData.stage = stage;  // ����SO�� �������� ���� ����?
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� MonsterDataSO_Test�� ���� 1.2f ���ϰ� ��Ʈ������ ��ȯ�ؼ� ����
        monsterData.Hp = Mathf.RoundToInt(monsterData.Hp * 1.2f);
        monsterData.Damage = Mathf.RoundToInt(monsterData.Damage * 1.2f);
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

//        // ���� ��Ȱ��ȭ �׽�Ʈ�� ���� �ڵ�(��ġ�� ���� ���� ����) 
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
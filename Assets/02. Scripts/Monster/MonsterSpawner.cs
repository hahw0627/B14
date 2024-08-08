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
            Debug.LogError("�������� ���� ����.");
            return;
        }

        bossMonster.SetActive(false);

        StartCoroutine(CheckMonsters());
    }

    
    // ���� ���� Ȯ�� + ��ȯ
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

    // ��� ���� ��Ȱ��ȭ Ȯ��
    private bool AllMonstersDeactivated()
    {
        foreach (GameObject monster in MonsterPool.Instance.monsters)
        {
            if (monster.activeSelf) return false;
        }
        return true;
    }

    // ���� ��ȯ
    private void SpawnMonsters()
    {
        // StagePage�� 1 �����ϸ� ������ �迭�� ��ġ���� ���͸� Ȱ��ȭ
        for (int i = 0; i < MonsterPool.Instance.monsters.Length; i++)
        {
            MonsterPool.Instance.monsters[i].transform.position = spawnPoints[i].position;
            //MonsterPool.Instance.monsters[i].GetComponent<Monster123>().target = target;
            MonsterPool.Instance.monsters[i].SetActive(true);
        }
    }

    // ���� ��ȯ
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
//            _monster.GetComponent<Monster>().MonsterStatistics = _monsterStatistics[(int)type];
//        }

//        private void Die()
//        {
//            MonsterPool.InsertQueue(gameObject);
//        }
//    }
//}
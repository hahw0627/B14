using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private List<MonsterStatistics> _monsterStatistics;

    [SerializeField]
    private List<Transform> _monsterPositions;

    [SerializeField]
    private GameObject _monsterPrefab;

    private const byte NUMBER_OF_MONSTERS = 6;

    private GameObject _monster;

    private void Start()
    {
        StartMonsterSpawn();
    }

    private void StartMonsterSpawn()
    {
        if (true)
        {
            for (var i = 0; i < NUMBER_OF_MONSTERS; i++)
            {
                SpawnMonster((MonsterType)StageTest.StageLevel,
                  new Vector3(_monsterPositions[i].position.x, _monsterPositions[i].position.y, 0.0f));
            }
        }
    }

    // 몬스터 비활성화 테스트를 위한 코드(합치고 나서 삭제 예정) 
    private void Update()
    {
        if (!PlayerTest.isAlive)
        {
            Die();
        }
    }

    private void SpawnMonster(MonsterType type, Vector3 position)
    {
        if (MonsterPool.Monsters.Count == 0)
        {
            _monster = Instantiate(_monsterPrefab, position, Quaternion.identity);
            //MonsterPool.Monsters.Enqueue(_monster.gameObject);
        }
        else
        {
            _monster = MonsterPool.GetQueue();
        }
        _monster.GetComponent<Monster>().MonsterStatistics = _monsterStatistics[(int)type];
    }

    private void Die()
    {
        MonsterPool.InsertQueue(gameObject);
    }
}
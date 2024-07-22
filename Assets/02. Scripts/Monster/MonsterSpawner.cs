using System.Collections.Generic;
using UnityEngine;

namespace _02._Scripts.Monster
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<MonsterStatistics> _monsterStatistics;

        [SerializeField]
        private List<Transform> _monsterPositions;

        [SerializeField]
        private GameObject _monsterPrefab;

        private GameObject _monster;

        private void Start()
        {
            StartMonsterSpawn();
        }

        private void StartMonsterSpawn()
        {
            foreach (Transform t in _monsterPositions)
            {
                SpawnMonster((MonsterType)StageTest.StageLevel,
                    new Vector3(t.position.x, t.position.y, 0.0f));
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
            _monster = MonsterPool.Monsters.Count == 0 ? Instantiate(_monsterPrefab, position, Quaternion.identity) :
                //MonsterPool.Monsters.Enqueue(_monster.gameObject);
                MonsterPool.GetQueue();
            _monster.GetComponent<global::Monster>().MonsterStatistics = _monsterStatistics[(int)type];
        }

        private void Die()
        {
            MonsterPool.InsertQueue(gameObject);
        }
    }
}
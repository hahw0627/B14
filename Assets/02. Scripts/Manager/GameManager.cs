using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<int, int> onStageChanged;
    public MonsterSpawner Spawner;

    public int StagePage
    {
        get => _stagePage;
        set
        {
            if (_stagePage == value) return;
            _stagePage = value;
            onStageChanged?.Invoke(_stage, _stagePage);
        }
    }

    public int Stage
    {
        get => _stage;
        set
        {
            if (_stage == value) return;
            _stage = value;
            onStageChanged?.Invoke(_stage, _stagePage);
        }
    }

    [SerializeField]
    private int _stagePage;

    [SerializeField]
    private int _stage = 1;
    
    private void Start()
    {
        StartCoroutine(CheckMonsters());
    }

    private IEnumerator CheckMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (Spawner.AllMonstersDeactivated())
            {
                StagePage++;
                switch (_stagePage)
                {
                    case <= 3:
                        Spawner.SpawnMonsters();
                        break;
                    case 4:
                        Spawner.SpawnBoss();
                        break;
                }
            }
        }
    }
}

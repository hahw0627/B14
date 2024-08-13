using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterPool : SingletonDestroyable<MonsterPool>
{
    public GameObject monsterPrefab;
    public GameObject[] monsters;

    private void Start()
    {
        monsters = new GameObject[6];
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i] = Instantiate(monsterPrefab, transform);
            monsters[i].GetComponent<Monster>().OnDeath += HandleMonsterDeath;
            monsters[i].SetActive(false);
        }
    }

    void HandleMonsterDeath(Monster monster123)
    {
        UIManager.Instance.UpdateCurrencyUI();
    }
}

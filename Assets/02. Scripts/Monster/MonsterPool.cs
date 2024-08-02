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
            monsters[i].GetComponent<Monster123>().OnDeath += HandleMonsterDeath;
            monsters[i].SetActive(false);
        }
    }

    // ?
    void HandleMonsterDeath(Monster123 monster123)
    {
        UIManager.Instance.UpdateCurrencyUI();
    }
}


//public static readonly Queue<GameObject> Monsters = new();

//public static void InsertQueue(GameObject monster)
//{
//    Monsters.Enqueue(monster);
//    monster.SetActive(false);
//}

//public static GameObject GetQueue()
//{ 
//    GameObject monster = Monsters.Dequeue();
//    monster.SetActive(true);

//    return monster;
//}
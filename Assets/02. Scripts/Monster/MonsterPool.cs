using System.Collections.Generic;
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

    // ?
    void HandleMonsterDeath(Monster monster123)
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
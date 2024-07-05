using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : SingletonDestroyable<MonsterPool>
{
    public static readonly Queue<GameObject> Monsters = new();
    
    public static void InsertQueue(GameObject monster)
    {
        Monsters.Enqueue(monster);
        monster.SetActive(false);
    }
    
    public static GameObject GetQueue()
    { 
        GameObject monster = Monsters.Dequeue();
        monster.SetActive(true);
          
        return monster;
    }
}

using UnityEngine;
using UnityEngine.Serialization;

public class MonsterPool : SingletonDestroyable<MonsterPool>
{
    public GameObject[] MonsterArray;

    [FormerlySerializedAs("monsters")]
    public GameObject[] Monsters;

    private void Start()
    {
        Monsters = new GameObject[6];
        for (int i = 0; i < Monsters.Length; i++)
        {
            Monsters[i] = Instantiate(MonsterArray[i], transform);
            Monsters[i].GetComponent<Monster>().onDeath += HandleMonsterDeath;
            Monsters[i].SetActive(false);
        }
    }

    public void HandleMonsterDeath(Monster monster123)
    {
        UIManager.Instance.UpdateCurrencyUI();
    }
}
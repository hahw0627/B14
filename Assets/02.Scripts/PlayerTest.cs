using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public static int MaxHp;
    public static int CurrentHp;
    public static bool isAlive;

    private void Start()
    {
        MaxHp = 100;
        CurrentHp = 100;
        isAlive = true;
    }
    
    private void Update()
    {
        if (CurrentHp > 0) return;
        Debug.Log("플레이어 사망");
        isAlive = false;
        Destroy(gameObject);
    }
}

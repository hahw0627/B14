using UnityEngine;

public enum EnemyType
{
    Easy,
    Normal,
    Hard,
    Boss
}

public class Enemy : MonoBehaviour
{
    private int _hp;
    public int Attack;

    public bool isAlive()
    {
        return _hp > 0;
    }
    
}
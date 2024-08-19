using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    [FormerlySerializedAs("target")]
    public Transform Target;

    public int Damage;

    [FormerlySerializedAs("speed")]
    public int Speed = 3;

    private Vector3 _direction;

    [FormerlySerializedAs("shooterTag")]
    public string ShooterTag;

    private SpriteRenderer _spriteRenderer;
    private int _cachedPlayerDamage;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        if (Target != null)
        {
            _direction = (Target.position - transform.position).normalized;
        }
    }

    private void OnEnable()
    {
        if (ShooterTag == "Player")
        {
            _cachedPlayerDamage = DataManager.Instance.PlayerDataSo.Damage;
        }

        StartCoroutine(DestroyAfterTime(1.5f));
    }

    private void Update()
    {
        transform.position += _direction * (Speed * Time.deltaTime);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((ShooterTag != "Player" || !collision.CompareTag("Monster")) &&
            (ShooterTag != "Monster" || !collision.CompareTag("Player"))) return;
        var currentDamage = ShooterTag == "Player" ? _cachedPlayerDamage : Damage;
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(currentDamage);
            }
        }
        else if (collision.CompareTag("Monster"))
        {
            var monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(currentDamage);
            }
        }

        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    public void SetDirection(Vector3 targetPosition)
    {
        _direction = (targetPosition - transform.position).normalized;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
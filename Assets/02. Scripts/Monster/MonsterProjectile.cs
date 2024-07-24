using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    public Transform target;
    public int damage;
    public int speed = 3;
    private Vector3 direction;

    private void Start()
    {
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }

        StartCoroutine(DestroyAfterTime(1f));
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("몬스터의 공격 적중");

        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

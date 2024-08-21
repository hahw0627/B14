using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;
    public GameObject projectilePrefab;
    public int poolSize = 10;
    private List<GameObject> pool;

    private void Awake()
    {
        Instance = this;
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetProjectile()
    {
        foreach (GameObject projectile in pool)
        {
            if (!projectile.activeInHierarchy)
            {
                projectile.SetActive(true);
                return projectile;
            }
        }

        // 필요한 경우 풀 크기를 늘릴 수 있음
        GameObject newProjectile = Instantiate(projectilePrefab, transform);
        newProjectile.SetActive(false);
        pool.Add(newProjectile);
        return newProjectile;
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
    }
}

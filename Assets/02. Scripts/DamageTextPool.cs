using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DamageTextPool : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private int poolSize = 10;
    private Queue<DamageText> pool;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        pool = new Queue<DamageText>();
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewDamageText();
        }
    }

    public DamageText GetDamageText()
    {
        if (pool.Count == 0)
        {
            CreateNewDamageText();
        }
        DamageText damageText = pool.Dequeue();
        damageText.gameObject.SetActive(true);
        damageText.Initialize(this);
        return damageText;
    }

    public void ReturnDamageText(DamageText damageText)
    {
        if (damageText != null)
        {
            damageText.gameObject.SetActive(false);
            pool.Enqueue(damageText);
        }
    }

    private void CreateNewDamageText()
    {
        if (damageTextPrefab == null)
        {
            Debug.LogError("DamageTextPrefab is not assigned in the inspector!");
            return;
        }

        GameObject newObject = Instantiate(damageTextPrefab, transform);
        DamageText damageText = newObject.GetComponent<DamageText>();
        if (damageText == null)
        {
            Debug.LogError("DamageText component not found on the instantiated prefab!");
            Destroy(newObject);
            return;
        }

        newObject.SetActive(false);
        damageText.Initialize(this);
        pool.Enqueue(damageText);
    }
}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class DamageTextPool : MonoBehaviour
{
    [FormerlySerializedAs("damageTextPrefab")]
    [SerializeField]
    private GameObject _damageTextPrefab;

    [FormerlySerializedAs("poolSize")]
    [SerializeField]
    private int _poolSize = 10;

    private Queue<DamageText> _pool;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _pool = new Queue<DamageText>();
        for (var i = 0; i < _poolSize; i++)
        {
            CreateNewDamageText();
        }
    }

    public DamageText GetDamageText()
    {
        if (_pool.Count == 0)
        {
            CreateNewDamageText();
        }

        var damageText = _pool.Dequeue();
        damageText.gameObject.SetActive(true);
        damageText.Initialize(this);
        return damageText;
    }

    public void ReturnDamageText(DamageText damageText)
    {
        if (damageText is null) return;
        damageText.gameObject.SetActive(false);
        _pool.Enqueue(damageText);
    }

    private void CreateNewDamageText()
    {
        if (_damageTextPrefab is null)
        {
            Debug.LogError("DamageTextPrefab is not assigned in the inspector!");
            return;
        }

        var newObject = Instantiate(_damageTextPrefab, transform);
        var damageText = newObject.GetComponent<DamageText>();
        if (damageText is null)
        {
            Debug.LogError("DamageText component not found on the instantiated prefab!");
            Destroy(newObject);
            return;
        }

        newObject.SetActive(false);
        damageText.Initialize(this);
        _pool.Enqueue(damageText);
    }
}
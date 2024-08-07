using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed; // �ؽ�Ʈ �̵��ӵ�

    [SerializeField]
    private float _alphaSpeed; // ���� ��ȯ�ӵ�

    [SerializeField]
    private float _destroyTime;

    [SerializeField]
    private int _damage;

    private TextMeshPro _text;
    private Color _alpha;


    public void SetDamage(int damageValue)
    {
        _damage = damageValue;
        if (_text is not null)
        {
            _text.text = _damage.ToString();
        }
    }

    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
        SetDamage(_damage);
        _alpha = _text.color;
        Invoke(nameof(DestroyObject), _destroyTime);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));
        _alpha.a = Mathf.Lerp(_alpha.a, 0, Time.deltaTime * _alphaSpeed);
        _text.color = _alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
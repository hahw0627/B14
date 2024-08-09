using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _alphaSpeed;
    [SerializeField] private float _activeTime;
    [SerializeField] private int _damage;

    private TextMeshPro _text;
    private Color _alpha;
    private DamageTextPool _pool;
    private float _timer;

    public void Initialize(DamageTextPool pool)
    {
        _pool = pool;
    }

    public void SetDamage(int damageValue)
    {
        _damage = damageValue;
        if (_text != null)
        {
            _text.text = _damage.ToString();
        }
    }

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        SetDamage(_damage);
        _alpha = _text.color;
        _alpha.a = 1f;
        _text.color = _alpha;
        _timer = 0f;
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));
        _alpha.a = Mathf.Lerp(_alpha.a, 0, Time.deltaTime * _alphaSpeed);
        _text.color = _alpha;

        _timer += Time.deltaTime;
        if (_timer >= _activeTime)
        {
            _pool.ReturnDamageText(this);
        }
    }
}

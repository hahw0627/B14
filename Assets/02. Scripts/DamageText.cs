using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _alphaSpeed;
    [SerializeField] private float _activeTime;
    [SerializeField] private int _damage;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _criticalColor = Color.red;
    [SerializeField] private float _criticalScale = 1.2f;

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
        SetDamage(damageValue, false);  // 기본적으로 치명타가 아닌 것으로 처리
    }

    public void SetDamage(int damageValue, bool isCritical = false, Color? customColor = null, float customScale = 1f)
    {
        if (_text == null) _text = GetComponent<TextMeshPro>();
        _text.text = damageValue.ToString();

        if (customColor.HasValue)
        {
            _text.color = customColor.Value;
        }
        else
        {
            _text.color = isCritical ? _criticalColor : _normalColor;
        }

        transform.localScale = Vector3.one * (isCritical ? _criticalScale : customScale);

        _alpha = _text.color;
        _alpha.a = 1f;
        _text.color = _alpha;
        _timer = 0f;
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

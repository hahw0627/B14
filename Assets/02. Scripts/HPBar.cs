using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class HpBar : MonoBehaviour
{
    public Slider Slider;
    public Transform Target;

    [FormerlySerializedAs("_maxHp")]
    public float MaxHp;

    [SerializeField]
    private TextMeshProUGUI _gageTMP;

    private Camera _camera;
    private float _currentHp;


    private void Awake()
    {
        _camera = Camera.main;
    }


    public void SetMaxHp(float maxHp)
    {
        if (Slider is null) return;
        MaxHp = maxHp;
        Slider.maxValue = maxHp;
    }

    public void SetCurrentHp(float currentHp)
    {
        if (Slider is null) return;
        _currentHp = currentHp;
        Slider.value = _currentHp;
        if(_currentHp < 0)
        {
            _currentHp = 0;
        }
        _gageTMP.text = $"{Mathf.Round(_currentHp / MaxHp * 100)}%".ToString();
    }

    private void Update()
    {
        if (Target is null) return;
        Debug.Assert(_camera is not null, "Camera.main != null");
        var screenPosition = _camera.WorldToScreenPoint(Target.position + new Vector3(0, 1.24f, 0));
        transform.position = screenPosition;
    }
}
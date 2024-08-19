using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class HpBar : MonoBehaviour
{
    public Slider Slider;
    public Transform Target;
    private Camera _camera;

    public float _maxHp;
    private float _currentHp;

    private void Awake()
    {
        _camera = Camera.main;
    }


    public void SetMaxHp(float maxHp)
    {
        if (Slider is null) return;
        _maxHp = maxHp;
        Slider.maxValue = maxHp;
    }

    public void SetCurrentHp(float currentHp)
    {
        if (Slider is null) return;
        _currentHp = currentHp;
        Slider.value = _currentHp;
    }

    private void Update()
    {
        if (Target is null) return;
        Debug.Assert(_camera is not null, "Camera.main != null");
        var screenPosition = _camera.WorldToScreenPoint(Target.position + new Vector3(0, 1.2f, 0));
        transform.position = screenPosition;
    }
}
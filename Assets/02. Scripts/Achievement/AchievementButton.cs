using System;
using UnityEngine;
using UnityEngine.UI;

public class AchievementButton : MonoBehaviour
{
    private Button _button;
    private bool _buttonFlag;
    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();
        _buttonFlag = true;
    }

    private void Start()
    {
        _button.onClick.AddListener(TaskOnClick);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void TaskOnClick()
    {
        _button.interactable = !_buttonFlag;
        Debug.Log(_buttonFlag);
    }
}

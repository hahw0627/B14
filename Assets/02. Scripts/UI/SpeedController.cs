using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedController : MonoBehaviour
{
    public float normalSpeed = 1f;
    public float fastSpeed = 2f;
    public Button speedToggleButton;

    private bool isFastMode = false;

    private void Start()
    {
        // 버튼에 리스너 추가
        speedToggleButton.onClick.AddListener(ToggleSpeed);

        // 초기 속도 설정
        //Time.timeScale = normalSpeed;
    }

    private void ToggleSpeed()
    {
        isFastMode = !isFastMode;

        if (isFastMode)
        {
            Time.timeScale = fastSpeed;
            speedToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "x2";
        }
        else
        {
            Time.timeScale = normalSpeed;
            speedToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "x1";
        }
    }
}

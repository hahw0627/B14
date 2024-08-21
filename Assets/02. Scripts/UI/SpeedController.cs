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
        // ��ư�� ������ �߰�
        speedToggleButton.onClick.AddListener(ToggleSpeed);

        // �ʱ� �ӵ� ����
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

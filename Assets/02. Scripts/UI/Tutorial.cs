using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;
    [SerializeField] private Button button5;
    [SerializeField] private Button button6;

    [SerializeField, TextArea]
    private List<string> _descriptions;
    
    [SerializeField]
    private TextMeshProUGUI text;

    private void Start()
    {
        // �� ��ư�� Ŭ�� �̺�Ʈ�� ����մϴ�.
        button1.onClick.AddListener(() => ChangeText(button1));
        button2.onClick.AddListener(() => ChangeText(button2));
        button3.onClick.AddListener(() => ChangeText(button3));
        button4.onClick.AddListener(() => ChangeText(button4));
        button5.onClick.AddListener(() => ChangeText(button5));
        button6.onClick.AddListener(() => ChangeText(button6));
    }

    public void ChangeText(Button btn)
    {
        switch (btn.name)
        {
            case "Button1":
                text.text = _descriptions[0];
                break;
            case "Button2":
                text.text = _descriptions[1];
                break;
            case "Button3":
                text.text = _descriptions[2];
                break;
            case "Button4":
                text.text = _descriptions[3];
                break;
            case "Button5":
                text.text = _descriptions[4];
                break;
            case "Button6":
                text.text = _descriptions[5];
                break;
            default:
                text.text = "알 수 없는 버튼이 눌렸습니다.";
                break;
        }
    }
}

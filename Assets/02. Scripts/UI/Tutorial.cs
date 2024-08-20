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
                text.text = "������ ���� �ϰ� �ٽ� �����ϸ� �ð��� ����ؼ� ��ġ ������ �ݴϴ�.\n" +
                            "���� ���� Ư�� ��ȭ�� GemStone ������ ���� �� �ֽ��ϴ�.\n" +
                            "(���� ��û ������ ���� ���� �ð� ���� ��ư�� ��Ȱ��ȭ �˴ϴ�.)";
                break;
            case "Button2":
                text.text = "Gold�� ���� óġ, ��ġ ����, ������ ��ȯ �ý������� ȹ�� �����մϴ�.\n" +
                            "Gem�� ���� ���� óġ, ���� ����, ������ ��ȯ �ý������� ȹ�� �����մϴ�.";
                break;
            case "Button3":
                text.text = "�̱⸦ ���� ���� ��ų�� ���ᰡ ���� 5���� ���̸� ��ȭ�� �� �� �ֽ��ϴ�.";
                break;
            case "Button4":
                text.text = "�ɷ�ġ�� �� 6���Դϴ�. ��ư�� ���� �ݾ׸�ŭ ����Ͽ� �ش� ������ ��ȭ�մϴ�.\n" +
                            "��, ��ȭ�� ������ ��ȭ ����� ���� �þ�� ������ ���� ��ȭ�غ�����!";
                break;
            case "Button5":
                text.text = "��� ��ư�� ������ ������ �ӵ��� ������ �� �� ������ �ÿ����� ���� �� �ֽ��ϴ�.";
                break;
            case "Button6":
                text.text = "��ų�� �����ϰ� �ڵ� ��ų ��ư�� ������ �ڵ����� ��ų�� ���ư� ������ ������ �ݴϴ�.\n" +
                            "������ ��ų���� ������ �ڵ� ��ư�� ����Ͽ� ������ ���� ������� ����������.";
                break;
            default:
                text.text = "�� �� ���� ��ư�� ���Ƚ��ϴ�.";
                break;
        }
    }
}

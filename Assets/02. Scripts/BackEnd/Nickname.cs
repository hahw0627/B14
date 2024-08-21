using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nickname : LoginBase
{
    [System.Serializable]
    public class NicknameEvent : UnityEngine.Events.UnityEvent { }
    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField]
    Image imageNickname; //�г��� ���󺯰�
    [SerializeField]
    TMP_InputField inputFieldNickname; //�г��� �ʵ� �ؽ�Ʈ ���� ����

    [SerializeField]
    Button btnUpdateNickname;

    private void OnEnable()
    {
        //�г��� ���濡 ������ ���� �޽����� ����� ���¿���
        //�г��� ���� �˾��� �ݾҴٰ� �� �� �ֱ� EOansdp tkdxofmf chrlghk
        ResetUI(imageNickname);
        SetMessage("�г����� �Է��ϼ���");

    }
    public void OnclickUpdateNickname()
    {
        //�Ű������� �Է��� inputField UI�� ����� Message ���� �ʱ�ȭ
        ResetUI(imageNickname);
        //�ʵ尪�� ����ִ��� üũ
        if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname")) return;

        //�г��� ���� ��ư�� ��ȣ�ۿ� ��Ȱ��ȭ
        btnUpdateNickname.interactable = false;
        SetMessage("�г����� �������Դϴ�..");

        //�ڳ� ���� �г��� ����õ�
        UpdateNickname();
    }
    void UpdateNickname()
    {
        Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
        {
            btnUpdateNickname.interactable = true;

            //�г��� ���漺��
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldNickname.text}(��)�� �г����� ����Ǿ����ϴ�.");

                //�г��� ���濡 �������� �� onnicknameevent�� ��ϵǾ��ִ� �̺�Ʈ �޼ҵ� ȣ��
                onNicknameEvent.Invoke();
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 400: //�� �г��� Ȥ�� string.empty , 20�� �̻��� �г��� , �г��� �յڿ� ������ �ִ°��
                        message = "�г����� ����ų� | 20�� �̻��̰ų� | ��/�ڿ� ������ �ֽ��ϴ�.";
                        break;
                    case 409:// �̹� �ߺ��� �г���
                        message = "�̹� �����ϴ� �г���";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;

                }
                GuideForIncorrectlyEnteredData(imageNickname, message);
            }
        });
    }

}

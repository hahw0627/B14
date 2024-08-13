using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using TMPro;

public class FindID : LoginBase
{
    [SerializeField]
    Image imageEmail;
    [SerializeField]
    TMP_InputField inputFieldEmail;

    [SerializeField]
    Button btnFindID;

    public void OnClickFindID()
    {
        ResetUI(imageEmail);

        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

        //���� ���� �˻�
        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageEmail, "���� ������ �߸� �Ǿ����ϴ�. (ex. address@xx.xx )");
            return;
        }
        btnFindID.interactable = false;
        SetMessage("���� �߼����Դϴ�..");

        FindCustomID();

    }

    void FindCustomID()
    {
        //���̵� ���� �̸��Ϸ� ����
        Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
        {
            btnFindID.interactable = true;
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail.text}�ּҷ� ������ �߼��Ͽ����ϴ�");
            }
            else
            {
                string message = string.Empty;
                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 404: // �ش� �̸����� ���̸Ӱ� ���°��
                        message = "�ش� �̸����� ����ϴ� ����ڰ� �����ϴ�";
                        break;
                    case 429:
                        message = "24�ð� �̳��� 5ȸ �̻� ���̵�/��й�ȣ  ã�⸦ �õ��߽��ϴ�";
                        break;
                    default:
                        //sattuscode : 400 => ������Ʈ�� Ư�����ڰ� �߰��� ��� (�ȳ� ���� �̹߼۹� �����߻�)
                        message = callback.GetMessage();
                        break;

                }
                if (message.Contains("�̸���"))
                {
                    GuideForIncorrectlyEnteredData(imageEmail, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
    }
}
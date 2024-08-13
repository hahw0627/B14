using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using UnityEngine.UI;
using System;
public class FindPW : LoginBase
{
    [SerializeField]
    Image imageID;
    [SerializeField]
    TMP_InputField inputFieldID;
    [SerializeField]
    Image imageEmail;
    [SerializeField]
    TMP_InputField inputFieldEmail;

    [SerializeField]
    Button btnFindPW;

    public void OnclickFindPW()
    {
        ResetUI(imageID, imageEmail);

        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageEmail, "���� ������ �߸� �Ǿ����ϴ�. (ex. address@xx.xx )");
            return;
        }

        btnFindPW.interactable = false;
        SetMessage("���� �߼����Դϴ�..");

        FindCustomPW();
    }

    private void FindCustomPW()
    {
        //��й�ȣ�� �ʱ�ȭ �ϰ�, ���µ� ��й�ȣ ������ �̸��Ϸ� �߼�
        Backend.BMember.ResetPassword(inputFieldID.text, inputFieldEmail.text, callback =>
        {
            //��й�ȣ ã�� ��ư ��ȣ�ۿ� Ȱ��ȭ
            btnFindPW.interactable= true;

            //���� �߼� ����
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail} �ּҷ� ������ �߼��Ͽ����ϴ�");
            }
            else
            {
                string message;
                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 404: // �ش� ������ ���̸Ӱ�����
                        message = "�ش� �̸����� ����ϴ� ����ڰ� �����ϴ�.";
                        break;
                    case 429:
                        message = "24�ð� �̳��� 5ȸ �̻� ���̵�/��й�ȣ ã�⸦ �õ��߽��ϴ�.";
                        break;
                    default:
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

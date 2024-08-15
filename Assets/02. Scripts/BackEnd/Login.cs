using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : LoginBase
{
    [SerializeField]
    private Image imageID;                  //ID �ʵ� ���󺯰�
    [SerializeField]                    
    private TMP_InputField inputFieldID;    // ID �ʵ� �ؽ�Ʈ ���� ����
    [SerializeField]
    private Image imagePW;                  //PW �ʵ� ���� ����
    [SerializeField]
    private TMP_InputField inputFieldPW;    // PW �ʵ� �ؽ�Ʈ ���� ����

    [SerializeField]
    private Button btnLogin;

    public void OnClickLogin()
    {
        //�Ű� ������ �Է��� inputFieldUI�� ����� Message ���� �ʱ�ȭ
        ResetUI(imageID,imagePW);

        //�ʵ� ���� ��� �ִ��� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;

        //�α��� ��ư ��Ÿ���� ���ϵ��� ��ȣ�ۿ� ��Ȱ��ȭ
        btnLogin.interactable = false;

        //������ �α����� ��û�ϴ� ���� ȭ�鿡 ����ϴ� ���� ������Ʈ
        // ex) �α��� ���� �ؽ�Ʈ ��� , ��Ϲ��� ������ ȸ�� ��
        StartCoroutine(nameof(LoginProcess));

        //�ڳ� ���� �α��� �õ�
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    private void ResponseToLogin(string ID,string PW)
    {
        Backend.BMember.CustomLogin(ID, PW, callbak =>
        {
            StopCoroutine(nameof(LoginProcess));

            //�α��� ����
            if (callbak.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}�� ȯ���մϴ�");

                // �� �̵� or ����ȭ�� ���� �߰� ..
                SceneManager.LoadScene("DevScene2");
            }
            else
            {
                btnLogin.interactable = true;
                string message = string.Empty;
                switch (int.Parse(callbak.GetStatusCode())) //���� ��ȣ ������ ������ ��ȯ�� ���� ��ȣ�� ���� ����� ���� ������ message ������ ����
                {
                    case 401: //�������� �ʴ� ���̵� , �߸��� ��й�ȣ 
                        message = callbak.GetMessage().Contains("customID") ? "�������� �ʴ� ���̵��Դϴ�." : "�߸��� ��й�ȣ �Դϴ�";
                        break;
                    case 403: //���� or ����̽� ����
                        message = callbak.GetMessage().Contains("user") ? "���ܴ��� �����Դϴ�." : "���ܴ��� ����̽��Դϴ�.";
                        break;
                    case 410: //Ż�� ������
                        message = "Ż�� �������� �����Դϴ�.";
                        break;
                    default:
                        message = callbak.GetMessage();
                        break;

                }
                // statusCode 401 ���� "�߸��� ��й�ȣ �Դϴ�." �϶� �ʵ���� �����ϰ� ���� ���� ���
                if (message.Contains("��й�ȣ"))
                {
                    GuideForIncorrectlyEnteredData(imagePW, message);
                }
                else
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
            }
        }
        );
    }
    private IEnumerator LoginProcess()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            SetMessage($"�α��� ���Դϴ�...{time:F1}");
            yield return null;
        }
    }
}

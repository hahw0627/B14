using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;


public class UserInfo : MonoBehaviour
{
    [System.Serializable]
    public class UserInfoEvent : UnityEngine.Events.UnityEvent { }
    public UserInfoEvent onUserInfoEvent = new UserInfoEvent();

    //�α����� ���������� ������ ������ ����
    static UserInfoData data = new UserInfoData();
    public static UserInfoData Data => data;
    public void GetUserInfoFromBackend()
    {
        Backend.BMember.GetUserInfo(callback =>
        {
            if (callback.IsSuccess())
            {
                //json ������ �Ľ� ����
                try
                {
                    JsonData json = callback.GetReturnValuetoJSON()["row"];
                    data.gamerId = json["gamerId"].ToString();
                    data.countryCode = json["countryCode"]?.ToString();
                    data.nickname = json["nickname"]?.ToString();
                    data.inDate = json["inDate"].ToString();
                    data.emailForFindPassword = json["emailForFindPassword"]?.ToString();
                    data.subscriptionType = json["subscriptionType"].ToString();
                    data.federationId = json["federationId"]?.ToString();
                }
                catch(System.Exception e)
                {
                    //���� ���� �⺻ ���·� 
                    data.Reset();
                    //try- catch ���� ���
                    Debug.LogError(e);
                }
            }
            else
            {
                //���� ������ �⺻ ���·� ����
                //tip.�Ϲ������� �������� ���¸� ����� �⺻���� ������ �����صΰ� ���������϶� �ҷ��ͼ� ���
                data.Reset();
                Debug.LogError(callback.GetMessage());
            }
            // ���� ���� �ҷ����Ⱑ �Ϸ� ������ onUserInfoEvent�� ��ϵ��ִ� �̺�Ʈ �޼ҵ� ȣ��
            onUserInfoEvent.Invoke();
        });
    }
}

public class UserInfoData
{
    public string gamerId;       //���� ���̵�
    public string countryCode;  //�����ڵ�
    public string nickname; //�г���
    public string inDate; // ����������
    public string emailForFindPassword; //�̸��� �ּ�
    public string subscriptionType; //Ŀ���� , ������̼�Ÿ��
    public string federationId; //���� ���� ���̽��� ������̼� id

    //������ ����·� �ʱ�ȭ
    public void Reset()
    {
        gamerId = "offline";
        countryCode = "Unkown";
        nickname = "noName";
        inDate = string.Empty;
        emailForFindPassword = string.Empty;
        subscriptionType = string.Empty;
        federationId = string.Empty;  
    }
}

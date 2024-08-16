using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginBase : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMessage;

    ///<summary>
    ///�޽��� ���� , inputField ���� �ʱ�ȭ
    ///</summary>
    protected void ResetUI(params Image[] images)
    {
        textMessage.text = string.Empty;
        for (int i = 0; i < images.Length; ++i)
        {
            images[i].color = Color.white;
        }
    }

    ///<summary>
    ///�Ű������� �ִ� �������
    ///</summary>
    protected void SetMessage(string msg)
    {
        textMessage.text = msg;
    }

    ///<summary>
    ///�Ű������� �޾ƿ� �ʵ� ����� �޼����� ����
    ///</summary>
    protected void GuideForIncorrectlyEnteredData(Image image, string msg)
    {
        textMessage.text = msg;
        image.color = Color.red;
    }

    ///<summary>
    ///�ʵ� ���� ����ִ��� Ȯ�� 
    ///</summary>
    protected bool IsFieldDataEmpty(Image image, string field, string result)
    {
        if (field.Trim().Equals(""))
        {
            GuideForIncorrectlyEnteredData(image, $"{result} 필드를 채워주세요");
            return true;
        }

        return false;
    }
}
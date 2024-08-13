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
    ///메시지 내용 , inputField 색상 초기화
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
    ///매개변수에 있는 내용출력
    ///</summary>
    protected void SetMessage(string msg)
    {
        textMessage.text = msg;
    }
    ///<summary>
    ///매개변수로 받아온 필드 색상과 메세지를 변경
    ///</summary>
    protected void GuideForIncorrectlyEnteredData(Image image, string msg)
    {
        textMessage.text = msg;
        image.color = Color.red;
    }
    ///<summary>
    ///필드 값이 비어있는지 확인 
    ///</summary>
    protected bool IsFieldDataEmpty(Image image, string field, string result)
    {
        if (field.Trim().Equals(""))
        {
            GuideForIncorrectlyEnteredData(image, $"\"{result}\" 필드를 채워주세요");
            return true;
        }
        return false;
    }

}

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
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;
        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageEmail, "메일 형식이 잘못 되었습니다. (ex. address@xx.xx )");
            return;
        }
        btnFindPW.interactable = false;
        SetMessage("메일 발송중입니다..");
        FindCustomPW();
    }
    private void FindCustomPW()
    {
        //비밀번호를 초기화 하고, 리셋된 비밀번호 정보를 이메일로 발송
        Backend.BMember.ResetPassword(inputFieldID.text, inputFieldEmail.text, callback =>
        {
            //비밀번호 찾기 버튼 상호작용 활성화
            btnFindPW.interactable= true;
            //메일 발송 성공
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail} 주소로 메일을 발송하였습니다");
            }
            else
            {
                string message;
                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 404: // 해당 메일의 게이머가없음
                        message = "해당 이메일을 사용하는 사용자가 없습니다.";
                        break;
                    case 429:
                        message = "24시간 이내에 5회 이상 아이디/비밀번호 찾기를 시도했습니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }
                if (message.Contains("이메일"))
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
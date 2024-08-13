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

        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;

        //메일 형식 검사
        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageEmail, "메일 형식이 잘못 되었습니다. (ex. address@xx.xx )");
            return;
        }
        btnFindID.interactable = false;
        SetMessage("메일 발송중입니다..");

        FindCustomID();

    }

    void FindCustomID()
    {
        //아이디 정보 이메일로 전송
        Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
        {
            btnFindID.interactable = true;
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail.text}주소로 메일을 발송하였습니다");
            }
            else
            {
                string message = string.Empty;
                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 404: // 해당 이메일의 게이머가 없는경우
                        message = "해당 이메일을 사용하는 사용자가 없습니다";
                        break;
                    case 429:
                        message = "24시간 이내에 5회 이상 아이디/비밀번호  찾기를 시도했습니다";
                        break;
                    default:
                        //sattuscode : 400 => 프로젝트명에 특수문자가 추가된 경우 (안내 메일 미발송및 에러발생)
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
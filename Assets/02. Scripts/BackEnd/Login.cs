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
    private Image imageID;                  //ID 필드 색상변경
    [SerializeField]                    
    private TMP_InputField inputFieldID;    // ID 필드 텍스트 정보 추출
    [SerializeField]
    private Image imagePW;                  //PW 필드 색상 변경
    [SerializeField]
    private TMP_InputField inputFieldPW;    // PW 필드 텍스트 정보 추출

    [SerializeField]
    private Button btnLogin;

    public void OnClickLogin()
    {
        //매개 변수로 입력한 inputFieldUI의 색상과 Message 내용 초기화
        ResetUI(imageID,imagePW);

        //필드 값이 비어 있는지 체크
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;

        //로그인 버튼 연타하지 못하도록 상호작용 비활성화
        btnLogin.interactable = false;

        //서버에 로그인을 요청하는 동안 화면에 출력하는 내용 업데이트
        // ex) 로그인 관련 텍스트 출력 , 톱니바퀴 아이콘 회전 등
        StartCoroutine(nameof(LoginProcess));

        //뒤끝 서버 로그인 시도
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    private void ResponseToLogin(string ID,string PW)
    {
        Backend.BMember.CustomLogin(ID, PW, callbak =>
        {
            StopCoroutine(nameof(LoginProcess));

            //로그인 성공
            if (callbak.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}님 환영합니다");

                // 씬 이동 or 게임화면 오픈 추가 ..
                SceneManager.LoadScene("DevScene2");
            }
            else
            {
                btnLogin.interactable = true;
                string message = string.Empty;
                switch (int.Parse(callbak.GetStatusCode())) //에러 번호 정보를 정수로 변환후 에러 번호에 따라 출력할 에러 내용을 message 변수에 저장
                {
                    case 401: //존재하지 않는 아이디 , 잘못된 비밀번호 
                        message = callbak.GetMessage().Contains("customID") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호 입니다";
                        break;
                    case 403: //유저 or 디바이스 차단
                        message = callbak.GetMessage().Contains("user") ? "차단당한 유저입니다." : "차단당한 디바이스입니다.";
                        break;
                    case 410: //탈퇴 진행중
                        message = "탈퇴가 진행중인 유저입니다.";
                        break;
                    default:
                        message = callbak.GetMessage();
                        break;

                }
                // statusCode 401 에서 "잘못된 비밀번호 입니다." 일때 필드색을 변경하고 에러 내용 출력
                if (message.Contains("비밀번호"))
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
            SetMessage($"로그인 중입니다...{time:F1}");
            yield return null;
        }
    }
}

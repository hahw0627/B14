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

    //로그인한 유저정보를 저장할 데이터 변수
    static UserInfoData data = new UserInfoData();
    public static UserInfoData Data => data;
    public void GetUserInfoFromBackend()
    {
        Backend.BMember.GetUserInfo(callback =>
        {
            if (callback.IsSuccess())
            {
                //json 데이터 파싱 성공
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
                    //유저 정보 기본 상태로 
                    data.Reset();
                    //try- catch 에러 출력
                    Debug.LogError(e);
                }
            }
            else
            {
                //유저 정보를 기본 상태로 설정
                //tip.일반적으로 오프라인 상태를 대비해 기본적인 정보를 저장해두고 오프라인일때 불러와서 사용
                data.Reset();
                Debug.LogError(callback.GetMessage());
            }
            // 유저 정보 불러오기가 완료 됐을때 onUserInfoEvent에 등록돼있는 이벤트 메소드 호출
            onUserInfoEvent.Invoke();
        });
    }
}

public class UserInfoData
{
    public string gamerId;       //게임 아이디
    public string countryCode;  //국가코드
    public string nickname; //닉네임
    public string inDate; // 유저데이터
    public string emailForFindPassword; //이메일 주소
    public string subscriptionType; //커스텀 , 페더레이션타입
    public string federationId; //구글 애플 페이스북 페더레이션 id

    //정보를 빈상태로 초기화
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

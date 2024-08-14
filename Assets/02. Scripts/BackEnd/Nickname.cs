using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nickname : LoginBase
{
    [System.Serializable]
    public class NicknameEvent : UnityEngine.Events.UnityEvent { }
    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField]
    Image imageNickname; //닉네임 색상변경
    [SerializeField]
    TMP_InputField inputFieldNickname; //닉네임 필드 텍스트 정보 추출

    [SerializeField]
    Button btnUpdateNickname;

    private void OnEnable()
    {
        //닉네임 변경에 실패해 에러 메시지를 출력한 상태에서
        //닉네임 변경 팝업을 닫았다가 열 수 있기 EOansdp tkdxofmf chrlghk
        ResetUI(imageNickname);
        SetMessage("닉네임을 입력하세요");

    }
    public void OnclickUpdateNickname()
    {
        //매개변수로 입력한 inputField UI의 색상과 Message 내용 초기화
        ResetUI(imageNickname);
        //필드값이 비어있는지 체크
        if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname")) return;

        //닉네임 변경 버튼의 상호작용 비활성화
        btnUpdateNickname.interactable = false;
        SetMessage("닉네임을 변경중입니다..");

        //뒤끝 서버 닉네임 변경시도
        UpdateNickname();
    }
    void UpdateNickname()
    {
        Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
        {
            btnUpdateNickname.interactable = true;

            //닉네임 변경성공
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldNickname.text}(으)로 닉네임이 변경되었습니다.");

                //닉네임 변경에 선공했을 때 onnicknameevent에 등록되어있는 이벤트 메소드 호출
                onNicknameEvent.Invoke();
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 400: //빈 닉네임 혹은 string.empty , 20자 이상의 닉네임 , 닉네임 앞뒤에 공백이 있는경우
                        message = "닉네임이 비었거나 | 20자 이상이거나 | 앞/뒤에 공백이 있습니다.";
                        break;
                    case 409:// 이미 중복된 닉네임
                        message = "이미 존재하는 닉네임";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;

                }
                GuideForIncorrectlyEnteredData(imageNickname, message);
            }
        });
    }

}

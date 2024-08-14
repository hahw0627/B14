using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupUpdateProfileViewer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textNickname;
    [SerializeField]
    TextMeshProUGUI textUserID;

    public void UpdateNickName()
    {
        //닉네임이 없으면 userid 출력
        textNickname.text = UserInfo.Data.nickname == null?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
        textUserID.text = UserInfo.Data.gamerId;
    }
}

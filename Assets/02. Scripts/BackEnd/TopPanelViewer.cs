using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopPanelViewer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textNickname;

    public void UpdateNickName()
    {
        // 닉네임이 없으면 gameid 출력
        textNickname.text = UserInfo.Data.nickname == null?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }
}

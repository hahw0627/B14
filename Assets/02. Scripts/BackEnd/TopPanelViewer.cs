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
        // �г����� ������ gameid ���
        textNickname.text = UserInfo.Data.nickname == null?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }
}

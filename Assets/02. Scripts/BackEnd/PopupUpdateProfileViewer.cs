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
        //�г����� ������ userid ���
        textNickname.text = UserInfo.Data.nickname == null?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
        textUserID.text = UserInfo.Data.gamerId;
    }
}

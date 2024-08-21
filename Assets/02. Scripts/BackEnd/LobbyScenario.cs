using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScenario : MonoBehaviour
{
    [SerializeField]
    UserInfo user;

    private void Awake()
    {
        user.GetUserInfoFromBackend();
    }
}

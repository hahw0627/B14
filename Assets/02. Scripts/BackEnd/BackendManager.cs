using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        BackendSetting();
    }
    private void Update()
    {
       
    }
    void BackendSetting()
    {
        var bro = Backend.Initialize(); // 뒤끝 초기화 

        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " +bro);
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // statusCode 400대 에러발생 
        }
    }

}

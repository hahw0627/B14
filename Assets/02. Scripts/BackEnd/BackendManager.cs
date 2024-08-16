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
        var bro = Backend.Initialize(); // �ڳ� �ʱ�ȭ 

        if (bro.IsSuccess())
        {
            Debug.Log("초기화성공 : " +bro);
        }
        else
        {
            Debug.LogError("<color=red>초기화실패 : " + bro + "</color>"); // statusCode 400�� �����߻� 
        }
    }

}

using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;using Random = UnityEngine.Random;

public enum SpeechLength {
    SHORT,
    LONG,
}

public class PlayerSpeechBubble : SingletonDestroyable<PlayerSpeechBubble>
{
    [Header("Settings")]
    [SerializeField]
    private TextMeshProUGUI _speechText; // TextMeshProUGUI 컴포넌트를 연결할 변수

    [SerializeField]
    private float _speechDuration;
    
    [SerializeField]
    private float _typingSpeed; // 타이핑 속도


    private void Start()
    {
        _speechText.text = "";
    }

    public void ShowMessage(string message, SpeechLength speechLength)
    {
        StartCoroutine(TypeText(message, speechLength));
    }
    
    public void ShowMessage(List<string> messages, SpeechLength speechLength)
    {
        var randomIndex = Random.Range(0, messages.Count); // 배열의 길이 내에서 랜덤 인덱스 생성
        StartCoroutine(TypeText(messages[randomIndex], speechLength));
    }

    private IEnumerator TypeText(string message, SpeechLength speechLength)
    {
        _speechText.text = ""; // 텍스트 초기화
        var speechDuration = speechLength switch
        {
            SpeechLength.SHORT => 2f,
            SpeechLength.LONG => 4f,
            _ => 3f
        };

        foreach (var letter in message)
        {
            _speechText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(_typingSpeed); // 타이핑 속도에 따라 대기
        }
        
        yield return new WaitForSeconds(speechDuration);
        _speechText.text = "";
    }

}
using System.Collections;
using UnityEngine;
using TMPro;

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

    public void ShowMessage(string message)
    {
        StartCoroutine(TypeText(message));
    }
    
    public void ShowMessage(string[] messages)
    {
        var randomIndex = Random.Range(0, messages.Length); // 배열의 길이 내에서 랜덤 인덱스 생성
        StartCoroutine(TypeText(messages[randomIndex]));
    }

    private IEnumerator TypeText(string message)
    {
        _speechText.text = ""; // 텍스트 초기화

        foreach (var letter in message)
        {
            _speechText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(_typingSpeed); // 타이핑 속도에 따라 대기
        }

        yield return new WaitForSeconds(_speechDuration);
        _speechText.text = "";
    }

}
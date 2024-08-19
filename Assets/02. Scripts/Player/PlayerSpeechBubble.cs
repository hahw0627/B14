using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public enum SpeechLength
{
    Short,
    Long
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

    private Color _newTxtColor;
    public Transform Target;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Start()
    {
        _speechText.text = "";
    }

    private void Update()
    {
        if (Target is null) return;
        Debug.Assert(_camera is not null, "Camera.main != null");
        var screenPosition = _camera.WorldToScreenPoint(Target.position + new Vector3(0, 1.5f, 0));
        transform.position = screenPosition;
    }

    private Coroutine _coroutine;

    public void ShowMessage(string message, SpeechLength speechLength, string hexColor = "#FFFFFF")
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(TypeText(message, speechLength, hexColor));
    }

    public void ShowMessage(List<string> messages, SpeechLength speechLength)
    {
        var randomIndex = Random.Range(0, messages.Count); // 배열의 길이 내에서 랜덤 인덱스 생성
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(TypeText(messages[randomIndex], speechLength));
    }

    private IEnumerator TypeText(string message, SpeechLength speechLength, string hexColor = "#FFFFFF")
    {
        _speechText.text = ""; // 텍스트 초기화
        if (ColorUtility.TryParseHtmlString(hexColor, out _newTxtColor))
        {
            _speechText.color = _newTxtColor;
        }

        var speechDuration = speechLength switch
        {
            SpeechLength.Short => 2f,
            SpeechLength.Long => 4f,
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
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class IntroCutScene : MonoBehaviour
{
    [SerializeField]
    private GameObject _introCutSceneObject;

    [Header("Scene Objects")]
    [SerializeField]
    private GameObject _imageObject;

    [SerializeField]
    private GameObject _captionObject;

    [Header("Scene Resources")]
    [SerializeField]
    private List<Sprite> _images;

    [SerializeField, TextArea]
    private List<string> _captions;

    [Header("Scene Controller")]
    [SerializeField]
    private Button _nextButton;
    
    [Header("Settings")]
    public float TypingSpeed = 0.14f; // 글자 출력 속도
    
    private int _resourceIndex;
    private Coroutine _typingCoroutine;
    private TextMeshProUGUI _captionTmp;

    private void Awake()
    {
        _nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void Start()
    {
        _captionTmp = _captionObject.GetComponent<TextMeshProUGUI>();
        UpdateScene();
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        SoundManager.Instance.Play("IntroBackground", type: Define.Sound.Bgm);
        if(SoundManager.Instance == null)
        {
            Debug.Log("사운드 매니저 없음");
        }
        else
        {
            Debug.Log("사운드 매니저 있음");
        }
    }

    private void OnDisable()
    {
        _resourceIndex = 0;
        _imageObject.GetComponent<Image>().sprite = _images[_resourceIndex];
        _captionObject.GetComponent<TextMeshProUGUI>().text = _captions[_resourceIndex];
    }

    private void UpdateScene()
    {
        // 화면에 표시될 리소스가 남아있는지 여부
        if (_resourceIndex < _images.Count && _resourceIndex < _captions.Count) // 아직 남아있다면
        {
            _imageObject.GetComponent<Image>().sprite = _images[_resourceIndex];
            _captionObject.GetComponent<TextMeshProUGUI>().text = ""; // 캡션 초기화
            _typingCoroutine = StartCoroutine(TypeText()); // 타이핑 효과 시작
        }
        else // 남아 있지 않다면
        {
            // 모든 장면을 보여준 후의 처리 (예: 씬 전환 또는 종료)
            Debug.Log("<color=white>인트로: 모든 장면을 표시했습니다.</color>");
            SoundManager.Instance.Play("MainBackground", type: Define.Sound.Bgm);
            Time.timeScale = 1f;
            _introCutSceneObject.SetActive(false);
            FirstRunCheck.SaveKeyOfFirstRun();
        }
    }

    private void OnNextButtonClicked()
    {
        if (_typingCoroutine != null)
        {
            // 타이핑 효과가 진행 중이라면
            StopCoroutine(_typingCoroutine);
            _captionObject.GetComponent<TextMeshProUGUI>().text = _captions[_resourceIndex]; // 캡션을 모두 표시
            _typingCoroutine = null; // 코루틴 참조 초기화
        }
        else
        {
            // 타이핑 효과가 끝났다면 다음 리소스로 넘어감
            _resourceIndex++;
            UpdateScene();
        }
    }
    
    private IEnumerator TypeText()  // 타이핑 효과(자막)
    {
        foreach (var letter in _captions[_resourceIndex])
        {
            _captionTmp.text += letter; // 한 글자씩 추가
            // 공백인 경우에는 기다리지 않음
            if (letter != ' ')
            {
                yield return new WaitForSecondsRealtime(TypingSpeed); // 지정한 시간만큼 대기
            }
        }
        _typingCoroutine = null; // 코루틴 종료 후 참조 초기화
    }
}
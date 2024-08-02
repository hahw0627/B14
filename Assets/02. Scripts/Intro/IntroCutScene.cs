using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private List<string> _captions;

    [Header("Scene Controller")]
    [SerializeField]
    private Button _nextButton;

    private int _resourceIndex;

    // [Header("Settings")]
    // public float TypingSpeed = 0.1f; // 글자 출력 속도

    private void Awake()
    {
        _resourceIndex = 0;
        _nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void Start()
    {
        UpdateScene();
    }

    private void UpdateScene()
    {
        if (_resourceIndex < _images.Count && _resourceIndex < _captions.Count)
        {
            _imageObject.GetComponent<Image>().sprite = _images[_resourceIndex];
            _captionObject.GetComponent<TextMeshProUGUI>().text = _captions[_resourceIndex];
            // StartCoroutine(nameof(TypeText));
        }
        else
        {
            // 모든 장면을 보여준 후의 처리 (예: 씬 전환 또는 종료)
            Debug.Log("인트로: 모든 장면을 표시했습니다.");
            Time.timeScale = 1f;
            _introCutSceneObject.SetActive(false);
        }
    }

    private void OnNextButtonClicked()
    {
        _resourceIndex++;
        UpdateScene();
/*
        if (_captionObject.GetComponent<TextMeshProUGUI>().text == _captions[_resourceIndex])
        {
            StopCoroutine(nameof(TypeText));
            _resourceIndex++;
        }
        else
        {
            StopCoroutine(nameof(TypeText));
            _captionObject.GetComponent<TextMeshProUGUI>().text = _captions[_resourceIndex];
        }
        */
    }
    /*
    private IEnumerator TypeText()
    {
        foreach (var letter in _captions[_resourceIndex])
        {
            _captionObject.GetComponent<TextMeshProUGUI>().text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(TypingSpeed); // 지정한 시간만큼 대기
        }
        _nextButton.interactable = true; // 대사가 끝난 후 버튼 활성화
    }
    */
}
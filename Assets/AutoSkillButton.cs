using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoSkillButton : MonoBehaviour
{
    [SerializeField]
    private Button _autoSkillButton; // Auto skill 버튼

    [SerializeField]
    private Image _icon; // 아이콘 이미지

    [SerializeField]
    private TextMeshProUGUI _buttonText; // 버튼 텍스트

    private Coroutine _glowCoroutine;

    private void Start()
    {
        // 버튼 클릭 이벤트 설정
        _autoSkillButton.onClick.AddListener(OnAutoSkillButtonClicked);
    }

    private void OnAutoSkillButtonClicked()
    {
        // 이미 코루틴이 실행 중이라면 중단
        if (_glowCoroutine != null)
        {
            StopCoroutine(_glowCoroutine);
        }

        _glowCoroutine = StartCoroutine(GlowEffect());
    }

    private IEnumerator GlowEffect()
    {
        while (true) // 무한 루프
        {
            // 빛나는 효과를 위해 색상 변화
            const float duration = 1f; // 효과 지속 시간
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                var t = elapsedTime / duration;

                // 소용돌이 효과
                var color = new Color(1f, 1f, 1f, Mathf.PingPong(t * 3, 1)); // 알파 값 변화
                _icon.color = color; // 아이콘 색상 변경
                _buttonText.color = color; // 텍스트 색상 변경

                elapsedTime += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
        }
    }
}
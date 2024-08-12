using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class AdButtonManager : MonoBehaviour
{
    public TextMeshProUGUI CooldownText;

    public const int COOLDOWN_TIME = 5;
    private Button _rewardedAdButton;

    private void Start()
    {
        _rewardedAdButton = gameObject.GetComponent<Button>();
        CooldownText.gameObject.SetActive(false);
    }
    
    public void StartCooldown(float cooldownTime)
    {
        CooldownText.gameObject.SetActive(true); // 쿨다운 텍스트 활성화
        _rewardedAdButton.interactable = false;
        StartCoroutine(CooldownTimer(cooldownTime));
    }

    private IEnumerator CooldownTimer(float cooldownTime)
    {
        var remainingTime = cooldownTime;

        while (remainingTime > 0)
        {
            var minutes = (int)(remainingTime / 60); // 남은 분 계산
            var seconds = (int)(remainingTime % 60); // 남은 초 계산
            CooldownText.text = $"{minutes:D2}:{seconds:D2}"; // 분:초 형식으로 표시
            yield return new WaitForSeconds(1);
            remainingTime--;
        }

        _rewardedAdButton.interactable = true;
        CooldownText.gameObject.SetActive(false); // 쿨다운 텍스트 비활성화
    }
}
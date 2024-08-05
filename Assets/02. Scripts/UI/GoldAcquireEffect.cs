using UnityEngine;
using DG.Tweening;


public class GoldAcquireEffect : MonoBehaviour
{
    public RectTransform goldIconPrefab;
    public RectTransform canvasRectTransform;
    public RectTransform targetGoldUI;

    public void PlayGoldAcquireEffect(Vector2 startPosition, int goldAmount)
    {
        int iconCount = Mathf.Min(goldAmount, 10);  // 최대 10개의 아이콘만 생성
        Sequence mainSequence = DOTween.Sequence();

        for (int i = 0; i < iconCount; i++)
        {
            RectTransform goldIcon = Instantiate(goldIconPrefab, canvasRectTransform);
            goldIcon.position = startPosition;

            Sequence iconSequence = DOTween.Sequence();

            // 랜덤한 위치로 퍼지는 효과
            Vector2 randomOffset = Random.insideUnitCircle * 100f;
            iconSequence.Append(goldIcon.DOAnchorPos(goldIcon.anchoredPosition + randomOffset, 0.5f).SetEase(Ease.OutQuad));

            // 목표 UI로 이동
            iconSequence.Append(goldIcon.DOMove(targetGoldUI.position, 0.5f).SetEase(Ease.InQuad));

            // 효과 종료 후 오브젝트 제거
            iconSequence.OnComplete(() => Destroy(goldIcon.gameObject));

            mainSequence.Join(iconSequence);
        }


            mainSequence.OnComplete(() => {
                targetGoldUI.DOPunchScale(Vector3.one * 0.2f, 0.25f);
            });
    }
}
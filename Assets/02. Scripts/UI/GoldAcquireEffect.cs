using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class GoldAcquireEffect : MonoBehaviour
{
    [FormerlySerializedAs("goldIconPrefab")]
    public RectTransform GoldIconPrefab;

    [FormerlySerializedAs("canvasRectTransform")]
    public RectTransform CanvasRectTransform;

    [FormerlySerializedAs("targetGoldUI")]
    public RectTransform TargetGoldUI;

    // 이펙트 완료 이벤트
    public delegate void EffectCompletedHandler();

    public event EffectCompletedHandler OnEffectCompleted;

    public void PlayGoldAcquireEffect(Vector2 startPosition, int goldAmount)
    {
        var iconCount = Mathf.Min(goldAmount, 10); // �ִ� 10���� �����ܸ� ����
        var mainSequence = DOTween.Sequence();

        for (var i = 0; i < iconCount; i++)
        {
            var goldIcon = Instantiate(GoldIconPrefab, CanvasRectTransform);
            goldIcon.position = startPosition;

            var iconSequence = DOTween.Sequence();

            // ������ ��ġ�� ������ ȿ��
            var randomOffset = Random.insideUnitCircle * 100f;
            iconSequence.Append(goldIcon.DOAnchorPos(goldIcon.anchoredPosition + randomOffset, 0.5f)
                .SetEase(Ease.OutQuad));

            // ��ǥ UI�� �̵�
            iconSequence.Append(goldIcon.DOMove(TargetGoldUI.position, 0.5f).SetEase(Ease.InQuad));

            // ȿ�� ���� �� ������Ʈ ����
            iconSequence.OnComplete(() => Destroy(goldIcon.gameObject));

            mainSequence.Join(iconSequence);
        }


        mainSequence.OnComplete(() =>
        {
            TargetGoldUI.DOPunchScale(Vector3.one * 0.2f, 0.25f);
            OnEffectCompleted?.Invoke();
        });
    }
}
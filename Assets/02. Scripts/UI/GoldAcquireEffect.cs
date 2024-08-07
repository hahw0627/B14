using UnityEngine;
using DG.Tweening;


public class GoldAcquireEffect : MonoBehaviour
{
    public RectTransform goldIconPrefab;
    public RectTransform canvasRectTransform;
    public RectTransform targetGoldUI;

    public void PlayGoldAcquireEffect(Vector2 startPosition, int goldAmount)
    {
        int iconCount = Mathf.Min(goldAmount, 10);  // �ִ� 10���� �����ܸ� ����
        Sequence mainSequence = DOTween.Sequence();

        for (int i = 0; i < iconCount; i++)
        {
            RectTransform goldIcon = Instantiate(goldIconPrefab, canvasRectTransform);
            goldIcon.position = startPosition;

            Sequence iconSequence = DOTween.Sequence();

            // ������ ��ġ�� ������ ȿ��
            Vector2 randomOffset = Random.insideUnitCircle * 100f;
            iconSequence.Append(goldIcon.DOAnchorPos(goldIcon.anchoredPosition + randomOffset, 0.5f).SetEase(Ease.OutQuad));

            // ��ǥ UI�� �̵�
            iconSequence.Append(goldIcon.DOMove(targetGoldUI.position, 0.5f).SetEase(Ease.InQuad));

            // ȿ�� ���� �� ������Ʈ ����
            iconSequence.OnComplete(() => Destroy(goldIcon.gameObject));

            mainSequence.Join(iconSequence);
        }


            mainSequence.OnComplete(() => {
                targetGoldUI.DOPunchScale(Vector3.one * 0.2f, 0.25f);
            });
    }
}
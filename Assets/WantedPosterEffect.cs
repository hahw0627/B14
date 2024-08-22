using System.Collections;
using UnityEngine;

public class WantedPosterEffect : MonoBehaviour
{
    public float DisplayDuration = 2.0f;
    public float GrowDuration = 1.0f;
    public Vector3 TargetScale = new(1f, 1f, 1f);

    private Vector3 _initialScale;

    private void OnEnable()
    {
        _initialScale = Vector3.zero;
        transform.localScale = _initialScale;
        StartCoroutine(ShowPoster());
    }

    private IEnumerator ShowPoster()
    {
        float elapsedTime = 0;

        while (elapsedTime < GrowDuration)
        {
            var scale = Mathf.Lerp(0, 1, elapsedTime / GrowDuration);
            transform.localScale = _initialScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = TargetScale;

        yield return new WaitForSeconds(DisplayDuration);

        elapsedTime = 0;
        while (elapsedTime < GrowDuration)
        {
            var scale = Mathf.Lerp(1, 0, elapsedTime / GrowDuration);
            transform.localScale = TargetScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = _initialScale;

        gameObject.SetActive(false);
        //transform.localScale = Vector3.one;
    }
}
using System.Collections;
using UnityEngine;

public class AutoDeactivateOnActivate : MonoBehaviour
{
    // 비활성화할 시간 설정
    public float DeactivateTime = 3f;

    private void OnEnable()
    {
        // 오브젝트가 활성화될 때 코루틴 시작
        StartCoroutine(DeactivateAfterTime(DeactivateTime));
    }

    private IEnumerator DeactivateAfterTime(float time)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(time);

        // 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
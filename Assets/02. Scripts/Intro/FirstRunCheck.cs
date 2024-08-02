using UnityEngine;

public class FirstRunCheck : MonoBehaviour
{
    private const string FIRST_RUN_KEY = "FirstRun";

    private void Start()
    {
        Debug.Log(!PlayerPrefs.HasKey(FIRST_RUN_KEY));
        // PlayerPrefs에서 'FirstRun' 키 확인
        if (!PlayerPrefs.HasKey(FIRST_RUN_KEY))
        {
            // 처음 실행인 경우
            Time.timeScale = 0f;
            gameObject.SetActive(true);
            Debug.Log("인트로: 게임을 처음 실행합니다!");

            // 'FirstRun' 키를 설정하여 다음부터는 처음 실행이 아님을 표시
            PlayerPrefs.SetInt(FIRST_RUN_KEY, 1);
            PlayerPrefs.Save();
        }
        else
        {
            // 이미 실행된 경우
            gameObject.SetActive(false);
            Debug.Log("인트로: 게임을 이미 실행했습니다.");
        }
    }

    public void InitFirstRun()
    {
        PlayerPrefs.DeleteKey(FIRST_RUN_KEY);
    }
}
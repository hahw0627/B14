using System.Collections;
using _10._Externals.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

public class PlayerEnterCombat : MonoBehaviour
{
    private Vector3 _combatPosition;
    private AnimationManager _animationManager;

    public float MoveSpeed = 1f;
    public string DialogueText; // 대사 텍스트
    public Vector3 ArrivalPosition;

    private void Awake()
    {
        _animationManager = gameObject.GetComponent<AnimationManager>();
        _combatPosition = ArrivalPosition;
    }

    private void Start()
    {
        // 게임 첫 실행이 아니라면 실행하지 않음
        if (!FirstRunCheck.IsFirstRun) return;
        // 시작 시 플레이어를 화면 좌측 밖으로 이동
        transform.position = new Vector3(-4f, transform.position.y, transform.position.z);
        // 좌측 밖에서 전투 위치로 걷기 애니메이션과 함께 이동
        StartCoroutine(nameof(EnterCombat));
    }

    private IEnumerator EnterCombat()
    {
        // 지정된 전투 위치로 이동
        while (Vector3.Distance(transform.position, _combatPosition) > 0.1f)
        {
            // 걷는 애니메이션 재생
            _animationManager.SetState(CharacterState.Walk);
            transform.position = Vector3.MoveTowards(transform.position, _combatPosition, MoveSpeed * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }

        // 걷는 애니메이션 중지
        _animationManager.SetState(CharacterState.Idle);
        PlayerSpeechBubble.Instance.ShowMessage(DialogueText, SpeechLength.Long);
    }
}
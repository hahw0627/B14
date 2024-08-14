using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupController : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle1;

    [SerializeField]
    private Toggle _toggle2;

    private void Start()
    {
        // 초기 상태 설정
        _toggle1.isOn = false;
        _toggle2.isOn = false;

        // 토글의 onValueChanged 이벤트에 메서드 추가
        _toggle1.onValueChanged.AddListener(OnToggleValueChanged);
        _toggle2.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        // 현재 클릭된 토글이 활성화되면 나머지 토글 비활성화
        if (isOn)
        {
            if (_toggle1.isOn)
            {
                _toggle2.interactable = false;
            }
            else if (_toggle2.isOn)
            {
                _toggle1.interactable = false;
            }
        }
        else
        {
            if (!_toggle1.isOn)
            {
                _toggle2.interactable = true;
            }
            else if (!_toggle2.isOn)
            {
                _toggle1.interactable = true;
            }
        }
    }
}
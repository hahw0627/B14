using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

public class UITextInteration : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [System.Serializable]
    class OnclickEvent : UnityEvent { }

    //text ui�� Ŭ�������� ȣ���ϰ� ���� �޼ҵ� ���
    [SerializeField]
    OnclickEvent onclickEvent;

    //������ �ٲ�� ��ġ�� �Ǵ�textmeshProUGUI
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Bold;

    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onclickEvent?.Invoke();
    }
}

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

    //text ui를 클릭햇을때 호출하고 싶은 메소드 등록
    [SerializeField]
    OnclickEvent onclickEvent;

    //색상이 바뀌고 터치가 되는textmeshProUGUI
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

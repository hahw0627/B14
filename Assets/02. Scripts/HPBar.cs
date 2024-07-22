using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;
    public Transform target;

    public void SetMaxHP(int maxHP)
    {
        slider.maxValue = maxHP;
        slider.value = maxHP;
    }
    public void SetCurrentHP (int currentHP)
    {
        slider.value = currentHP;
    }
    private void Update()
    {
        if(target != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position + new Vector3(0,2,0));
            transform.position = screenPosition;
        }
    }
}

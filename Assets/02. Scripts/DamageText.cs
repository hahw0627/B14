using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class DamageText : MonoBehaviour
{
    public float moveSpeed; // 텍스트 이동속도
    public float alphaSpeed; // 투명도 변환속도
    public float destroyTime;
    TextMeshPro text;
    Color alpha;
    public int damage;

    public void SetDamage(int damageValue)
    {
        damage = damageValue;
        if( text != null)
        {
            text.text = damage.ToString();
        }
    }
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        SetDamage(damage);
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}

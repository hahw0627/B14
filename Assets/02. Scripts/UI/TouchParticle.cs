using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchParticle : MonoBehaviour, IPointerDownHandler
{
    public ParticleSystem clickParticle;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        touchPosition.z = 0;
        ParticleSystem newParticle = Instantiate(clickParticle, touchPosition, Quaternion.identity);

    }

}

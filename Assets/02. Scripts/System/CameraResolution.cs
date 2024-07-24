using UnityEngine;

namespace _02._Scripts.System
{
    public class CameraResolution : MonoBehaviour
    {
        private void Start()
        {
            var component = GetComponent<Camera>();
            Rect r = component.rect;
            var scaleHeight = ((float)Screen.width / Screen.height) / (9f / 20f);
            var scaleWidth = 1f / scaleHeight;
            if (scaleHeight < 1f)
            {
                r.height = scaleHeight;
                r.y = (1f - scaleHeight) / 2f;
            }
            else
            {
                r.width = scaleWidth;
                r.x = (1f - scaleWidth) / 2f;
            }

            component.rect = r;
        }

        void OnPreCull() => GL.Clear(true, true, Color.black);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.Unity.Utils
{
    public class SpriteScaler : MonoBehaviour
    {
        [SerializeField]
        private float percentage;

        private void Awake()
        {
            transform.localScale = GetScale() * percentage;
        }

        Vector2 GetFrameSize()
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float camHalfHeight = Camera.main.orthographicSize;
            float camHalfWidth = screenAspect * camHalfHeight;
            float camWidth = 2.0f * camHalfWidth;

            return new Vector2(camWidth, camHalfHeight * 2f);
        }

        Vector2 GetScale()
        {
            var camSize = GetFrameSize();
            //var spriteSize = GetComponent<SpriteRenderer>().size;
            var spriteSize = GetComponent<SpriteRenderer>().sprite.rect.size/100f;
            //Debug.Log(camSize + "  ~~~  " + spriteSize + "  " + (picelSize/100f));
            Vector2 scale;
            scale.x = camSize.x / spriteSize.x;
            scale.y = camSize.y / spriteSize.y;
            return scale;
        }
    }
}

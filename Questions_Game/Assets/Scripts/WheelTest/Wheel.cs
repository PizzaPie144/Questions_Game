using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace PizzaPie.QuestionsGame.Test
{
    public class Wheel : MonoBehaviour
    {
        Texture2D texture;
        int size;
        public float offset = 0.9f;
        public int pixelsPerUnit = 100;
        SpriteRenderer sr;

        private void Start()
        {
            InitTexture();
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit, 50, SpriteMeshType.FullRect);

        }

        private Texture2D InitTexture()
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float camHalfHeight = Camera.main.orthographicSize;
            float camHalfWidth = screenAspect * camHalfHeight;
            float camHeight = 2f * camHalfHeight;
            float camWidth = 2f * camHalfWidth;

            float refSize = 0f;
            if (camWidth < camHeight)
                refSize = camWidth;
            else
            {
                refSize = camHeight / 2f;
            }

            size = (int)((float)refSize * offset * (float)pixelsPerUnit);

            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.clear);
            tex.Resize(size, size);
            return tex;
        }

        private void UpdateTexture(List<float> percentages, List<Color> colors)
        {
            GenerateWheelTexture(texture, percentages, colors);
        }

        private async void GenerateWheelTexture(Texture2D texture, List<float> percentages, List<Color> colors)
        {
            int size = texture.width;
            Color[] texRaw = await Task.Run<Color[]>(() => GenenerateTexData(size, size, size / 2, percentages, colors));
            texture.SetPixels(texRaw);
            texture.Apply();
        }
        private Color[] GenenerateTexData(int width, int height, int radius, List<float> percentages, List<Color> palette)
        {
            Color[] tex = new Color[width * height];

            Vector2 up = Vector2.up;
            Vector2 center = Vector2.one * Mathf.FloorToInt((float)width / 2f);

            var radiusSqr = Mathf.Pow(((float)radius), 2);

            int index = 0;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (Mathf.Pow(((float)i) - center.x, 2) + Mathf.Pow(((float)j) - center.y, 2) <= radiusSqr)
                    {
                        var x = (float)i - center.x;
                        var y = (float)j - center.y;
                        var angle = Mathf.Atan2(x * up.y - y * up.x, x * up.x + y * up.y) * 180 / Mathf.PI;
                        angle += 180;
                        var minAngle = 0f;
                        var maxAngle = 0f;

                        for (int k = 0; k < percentages.Count; k++)
                        {
                            if (k != 0)
                                minAngle = maxAngle;

                            maxAngle += 360 * percentages[k];
                            if (minAngle < angle && maxAngle > angle)
                            {
                                tex[index] = palette[k];
                                break;
                            }
                        }
                    }
                    else
                        tex[index] = Color.clear;
                    index++;

                }
            }

            return tex;
        }
    }
}
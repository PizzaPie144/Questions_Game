using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.IO;


namespace PizzaPie.Editor.Test
{
    public class TextureGenerator : EditorWindow
    {
        int size = 2;
        float offset = 0.9f;
        Texture2D texture;
        int pixelsPerUnit = 100;
        List<float> percentages = new List<float>() { 0.25f, 0.25f, 0.25f, 0.25f };
        List<Color> colors = new List<Color>() { Color.red, Color.cyan, Color.green, Color.yellow };

        string path = "aaa.png";
        [MenuItem("Window/Texture Generate")]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<TextureGenerator>();
            window.Show();
        }


        private void OnGUI()
        {
            if (GUILayout.Button("Generate"))
            {
                GenTexureFile();
            }
        }

        private void GenTexureFile()
        {
            texture = InitTexture();
            UpdateTexture(percentages, colors);
            ImageConversion.EncodeToPNG(texture);
            var raw = ImageConversion.EncodeToPNG(texture);//texture.EncodeToPNG();
            var filePath = Path.Combine(Application.dataPath, path);
            File.Delete(filePath);
            File.WriteAllBytes(filePath, raw);
            AssetDatabase.Refresh();
        }

        private Texture2D InitTexture()
        {
            float refSize = 10f;

            size = (int)((float)refSize * offset * (float)pixelsPerUnit);

            Texture2D tex = new Texture2D(1, 1,TextureFormat.RGBA32,false);
            tex.SetPixel(0, 0, Color.clear);
            tex.Resize(size, size);
            return tex;
        }

        private void UpdateTexture(List<float> percentages, List<Color> colors)
        {
            GenerateWheelTexture(texture, percentages, colors);
        }

        private /*async*/ void GenerateWheelTexture(Texture2D texture, List<float> percentages, List<Color> colors)
        {
            int size = texture.width;
            Color[] texRaw = GenenerateTexData(size, size, size / 2, percentages, colors);//await Task.Run<Color[]>(() => GenenerateTexData(size, size, size / 2, percentages, colors));
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
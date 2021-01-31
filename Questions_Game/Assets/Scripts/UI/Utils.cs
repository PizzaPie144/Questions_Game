using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PizzaPie.QuestionsGame.UI
{
    public static class Utils
    {
        public static IEnumerator ButtonFlash(Button button, ColorBlock defaultColorBlock, Color color, float delay, int repeats)
        {
            ColorBlock colorBlock = defaultColorBlock;
            var targetColorBlock = new ColorBlock();
            targetColorBlock.normalColor = color;
            targetColorBlock.disabledColor = color;
            targetColorBlock.highlightedColor = color;
            targetColorBlock.pressedColor = color;

            var defaultColor = colorBlock.normalColor;

            for (int i = 0; i < repeats; i++)
            {
                var t = i % 2 == 1 ? delay : 0f;
                do
                {
                    t = i % 2 == 1 ? t - Time.deltaTime : t + Time.deltaTime;
                    var targetColor = Vector4.Lerp(defaultColor, color, t / delay);
                    
                    colorBlock.normalColor = targetColor;
                    colorBlock.disabledColor = targetColor;
                    colorBlock.highlightedColor = targetColor;
                    colorBlock.pressedColor = targetColor;

                    button.colors = colorBlock;

                    yield return null;
                } while (t <= delay && t >= 0);
            }
        }

        public static IEnumerator CanvasGroupFade(CanvasGroup canvasGroup, float duration, float targetA)
        {
            var t = 0f;
            var currentA = canvasGroup.alpha;
            do
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(currentA, targetA, t / duration);
                yield return null;
            } while (t <= duration);
        }
    }
}
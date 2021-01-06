using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaPie.Unity.Utils;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Button button;
    private void Start()
    {

        //StartCoroutine(PizzaPie.UI.Utils.CanvasGroupFade(canvasGroup, 3f, 1));
        button.onClick.AddListener(SSS);
    }

    void SSS()
    {
        StartCoroutine(PizzaPie.UI.Utils.ButtonFlash(button, button.colors, Color.red, 0.5f, 10));
    }

    IEnumerator AAA()
    {
        Debug.Log("AAA");
        yield return new WaitForSeconds(2f);
    }
    IEnumerator BBB()
    {
        Debug.Log("BBB");
        yield return new WaitForSeconds(2f);
    }
    IEnumerator CCC()
    {
        Debug.Log("CCC");
        yield return new WaitForSeconds(2f);
    }
    IEnumerator DDD()
    {
        Debug.Log("DDD");
        yield return new WaitForSeconds(2f);
    }
}

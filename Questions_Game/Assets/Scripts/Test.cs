using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PizzaPie.QuestionsGame.Unity.Utils;

public class Test : MonoBehaviour
{
    

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

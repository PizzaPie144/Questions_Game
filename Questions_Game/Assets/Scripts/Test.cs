using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaPie.Unity.Utils;

public class Test : MonoBehaviour
{
    private void Start()
    {
        SequenceLoader sequenceLoader = new SequenceLoader();

        sequenceLoader.AddEnumerator(AAA());
        sequenceLoader.AddEnumerator(BBB());
        sequenceLoader.AddEnumerator(CCC());
        sequenceLoader.AddEnumerator(DDD());

        sequenceLoader.Run();

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

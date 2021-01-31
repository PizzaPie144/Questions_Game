using UnityEngine;

namespace PizzaPie.QuestionsGame.Unity.Utils
{
    public class CoroutinesHandler : MonoBehaviour
    {
        public static CoroutinesHandler Create()
        {
            var go = new GameObject("Coroutines Handler");
            return go.AddComponent<CoroutinesHandler>();
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
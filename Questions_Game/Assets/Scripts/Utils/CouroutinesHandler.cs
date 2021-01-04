using UnityEngine;

namespace PizzaPie.Unity.Utils
{
    public class CouroutinesHandler : MonoBehaviour
    {
        public static CouroutinesHandler Init()
        {
            var go = new GameObject("Coroutines Handler");
            return go.AddComponent<CouroutinesHandler>();
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
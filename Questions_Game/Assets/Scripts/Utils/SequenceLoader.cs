using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.Unity.Utils
{
    public class SequenceLoader
    {
        private CoroutinesHandler coroutinesHandler;
        private List<IEnumerator> enumerators = new List<IEnumerator>();
        private System.Action OnFinish;
        private bool disposeOnFinish;


        public SequenceLoader(System.Action OnFinish = null, bool disposeOnFinish = true)
        {
            coroutinesHandler = CoroutinesHandler.Create();
            this.OnFinish += OnFinish;
            this.disposeOnFinish = disposeOnFinish;
        }

        public void AddOnFinishAction(System.Action OnFinish)
        {
            this.OnFinish += OnFinish;
        }
        
        public void AddEnumerator(IEnumerator enumerator)
        {
            enumerators.Add(enumerator);
        }

        public void RemoveEnumerator(IEnumerator enumerator)
        {
            enumerators.Remove(enumerator);
        }

        public void Start()
        {
            coroutinesHandler.StartCoroutine(SequenceRoutine(enumerators.ToArray()));
        }

        public void AbortSequence()
        {
            coroutinesHandler.StopAllCoroutines();
        }

        public void Reset()
        {
            AbortSequence();
            enumerators = new List<IEnumerator>();
        }

        public void Dispose()
        {
            AbortSequence();
            coroutinesHandler.Dispose();
        }

        private IEnumerator SequenceRoutine(params IEnumerator[] enumerators)
        {
            foreach (var en in enumerators)
                yield return coroutinesHandler.StartCoroutine(en);

            if (OnFinish != null)
                OnFinish();

            if (disposeOnFinish)
                Dispose();
        }

    }
}
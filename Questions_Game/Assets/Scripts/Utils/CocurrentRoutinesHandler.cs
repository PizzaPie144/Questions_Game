using System.Collections;
using System.Collections.Generic;
using System;

namespace PizzaPie.Unity.Utils
{
    public class CocurrentRoutineHandler
    {
        private CoroutinesHandler coroutinesHandler;
        List<Func<IEnumerator>> enumerators;
        List<bool> routinesRecord;

        //private Dictionary<int, bool> routinesRunningRecord;


        private bool disposeOnFinish;
        int index = 0;

        private System.Action OnFinishAction;

        public CocurrentRoutineHandler(System.Action OnFinishAction = null, bool disposeOnFinish = true, params Func<IEnumerator>[] routines)
        {
            coroutinesHandler = CoroutinesHandler.Create();
            AddOnFinishCallback(OnFinishAction);
            this.disposeOnFinish = disposeOnFinish;
            enumerators = new List<Func<IEnumerator>>();
            routinesRecord = new List<bool>();
            foreach (var routine in routines)
            {
                enumerators.Add(routine);
                routinesRecord.Add(false);

                index++;
            }
        }

        public void Start()
        {
            coroutinesHandler.StartCoroutine(CoccurentMainRoutine());
        }

        public void AddOnFinishCallback(System.Action OnFinish)
        {
            if (OnFinishAction != null)
                OnFinishAction += OnFinish;
            else
                OnFinishAction = OnFinish;
        }

        private IEnumerator CoccurentMainRoutine()
        {
            for (int i = 0; i < routinesRecord.Count; i++)
                coroutinesHandler.StartCoroutine(TrackRoutine(i));

            while (GetCheckSum(routinesRecord))
                yield return null;

            if (OnFinishAction != null)
                OnFinishAction();

            if (disposeOnFinish)
                Dispose();

        }

        private IEnumerator TrackRoutine(int index)
        {
            routinesRecord[index] = true;
            yield return coroutinesHandler.StartCoroutine(enumerators[index]());
            routinesRecord[index] = false;
        }

        private bool GetCheckSum(List<bool> record)
        {
            bool sum = false;
            foreach (var routine in record)
                sum |= routine;

            return sum;
        }

        public void AddRoutine(Func<IEnumerator> routine)
        {
            routinesRecord.Add(false);
            enumerators.Add(routine);
            index++;
        }

        public void Abort()
        {
            coroutinesHandler.StopAllCoroutines();
        }

        public void Dispose()
        {
            enumerators.Clear();
            coroutinesHandler.Dispose();
        }
    }
}

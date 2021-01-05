using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.Unity.Utils
{
    public class CocurrentRoutineHandler
    {
        private CouroutinesHandler coroutinesHandler;
        private Dictionary<IEnumerator, bool> routinesRunningRecord;

        private bool disposeOnFinish;
        private System.Action OnFinishAction;

        public CocurrentRoutineHandler(System.Action OnFinishAction = null, bool disposeOnFinish = true, params IEnumerator[] routines)
        {
            AddOnFinishCallback(OnFinishAction);
            this.disposeOnFinish = disposeOnFinish;

            routinesRunningRecord = new Dictionary<IEnumerator, bool>();
            foreach (var routine in routines)
                routinesRunningRecord.Add(routine, false);
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
            foreach (var routine in routinesRunningRecord)
                coroutinesHandler.StartCoroutine(TrackRoutine(routine.Key));

            while (GetCheckSum(routinesRunningRecord))
                yield return null;

            if (OnFinishAction != null)
                OnFinishAction();

            if (disposeOnFinish)
                Dispose();
        }

        private IEnumerator TrackRoutine(IEnumerator routine)
        {
            routinesRunningRecord[routine] = true;
            yield return coroutinesHandler.StartCoroutine(routine);
            routinesRunningRecord[routine] = false;
        }

        private bool GetCheckSum(Dictionary<IEnumerator,bool> record)
        {
            bool sum = true;
            foreach (var routine in record)
                sum |= routine.Value;

            return sum;
        }

        public void AddRoutine(IEnumerator routine)
        {
            routinesRunningRecord.Add(routine, false);
        }

        public void RemoveRoutine(IEnumerator routine)
        {
            if (routinesRunningRecord.ContainsKey(routine))
                routinesRunningRecord.Remove(routine);
        }

        public void Abort()
        {
            coroutinesHandler.StopAllCoroutines();
        }

        public void Clear()
        {
            routinesRunningRecord.Clear();
        }

        public void Dispose()
        {
            routinesRunningRecord = null;
            coroutinesHandler.Dispose();
        }
    }
}

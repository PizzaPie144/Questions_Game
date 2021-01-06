using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaPie.Events;
using PizzaPie.UI;
using PizzaPie.Questions;

namespace PizzaPie
{
    public class GameManager : MonoBehaviour , ISubscriber<DifficultySelectedEventArgs> ,ISubscriber<AnswerPickedCoccurentEventArgs> ,ISubscriber<PlayAgainEventArgs>
    {
        private DifficultyDefinition selectedDifficulty;
 
        private int wrongAnswersCount;
        private int rightAnswersCount;

        [SerializeField]
        private float minLoadTime;

        private void Start()
        {
            Services.Instance.EventAggregator.Subscribe<DifficultySelectedEventArgs>(this);
            Services.Instance.EventAggregator.Subscribe<AnswerPickedCoccurentEventArgs>(this);
            Services.Instance.EventAggregator.Subscribe<PlayAgainEventArgs>(this);
        }

        
        #region event handlers

        public void Handler(object sender, DifficultySelectedEventArgs e)
        {
            e.CoccurentRoutine.AddOnFinishCallback(InitLoad);
            selectedDifficulty = e.DifficultyDefinition;
        }


        public void Handler(object sender, AnswerPickedCoccurentEventArgs e)
        {
            if (e.IsRight)
                rightAnswersCount++;
            else
                wrongAnswersCount++;

            if (selectedDifficulty.IsWin(rightAnswersCount) || selectedDifficulty.IsLose(wrongAnswersCount))
            {
                e.CocurrentRoutineHandler.AddOnFinishCallback(() => OnEnd(e.IsRight));
            }
            else
                e.CocurrentRoutineHandler.AddOnFinishCallback(() => Services.Instance.EventAggregator.Invoke(this, new GameStartsEventArgs()));

        }

        public void Handler(object sender, PlayAgainEventArgs e)
        {
            _Reset();
            Services.Instance.EventAggregator.Invoke(this, new PlayButtonEventArgs());
        }

        #endregion

        private void OnEnd(bool isWin)
        {
            var cocurrentRoutine = new Unity.Utils.CocurrentRoutineHandler();
            Services.Instance.EventAggregator.Invoke(this, new EndGameCocurrentEventArgs(isWin, cocurrentRoutine));
            cocurrentRoutine.Start();
        }

        private void InitLoad()
        {
            var sequence = new Unity.Utils.SequenceLoader();
            sequence.AddOnFinishAction(() => Services.Instance.EventAggregator.Invoke(this, new GameStartsEventArgs()));
            Services.Instance.EventAggregator.Invoke(this, new LoadEventArgs(sequence, selectedDifficulty.GameDifficulty));
            sequence.AddEnumerator(MinDelayForceRoutine(minLoadTime, Time.time));
            sequence.Start();
        }


        private void _Reset()
        {
            rightAnswersCount = wrongAnswersCount = 0;
            selectedDifficulty = null;
        }

        private IEnumerator MinDelayForceRoutine(float delay, float timeInitiated)
        {
            var timeLimit = timeInitiated + delay;

            if (timeLimit <= Time.time)
                yield break;

            yield return new WaitForSeconds(timeLimit - Time.time );
        }

        
    }
}


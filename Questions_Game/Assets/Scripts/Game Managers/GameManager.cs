using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaPie.Events;
using PizzaPie.UI;
using PizzaPie.Questions;

namespace PizzaPie
{
    public class GameManager : MonoBehaviour , ISubscriber<DifficultySelectedEventArgs> ,ISubscriber<AnswerPickedCoccurentEventArgs>
    {
        private DifficultyDefinition selectedDifficulty;
 
        private int wrongAnswersCount;
        private int rightAnswersCount;

        private void Start()
        {
            Services.Instance.EventAggregator.Subscribe<DifficultySelectedEventArgs>(this);
            Services.Instance.EventAggregator.Subscribe<AnswerPickedCoccurentEventArgs>(this);
        }

        private void OnDestroy()
        {
            Services.Instance.EventAggregator.Unsubscribe<DifficultySelectedEventArgs>(this);
            Services.Instance.EventAggregator.Unsubscribe<AnswerPickedCoccurentEventArgs>(this);
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
            {
                if (selectedDifficulty.IsWin(++rightAnswersCount))
                {
                    //Init win sequence 
                }
                else
                {
                    Services.Instance.EventAggregator.Invoke(this,new GameStartsEventArgs());
                }
            }
            else
            {
                if (selectedDifficulty.IsLose(++wrongAnswersCount))
                {
                    //init lose sequence
                }
                else
                {
                    Services.Instance.EventAggregator.Invoke(this, new GameStartsEventArgs());
                }
            }
        }
        #endregion

        private void InitLoad()
        {
            var sequence = new Unity.Utils.SequenceLoader();
            Services.Instance.EventAggregator.Invoke(this, new BeforeLoadEventArgs(sequence, selectedDifficulty.GameDifficulty));
            sequence.AddEnumerator(MinDelayForceRoutine(5f, Time.time));
            sequence.Start();
        }

        //OnAnswerPickedcoccurent (though will only need the OnFinish Function del)
        private void @Reset()
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


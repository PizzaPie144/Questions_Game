using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PizzaPie.Questions;
using PizzaPie.Events;

namespace PizzaPie.UI
{
    public class QuestionUIManager : MonoBehaviour, ISubscriber<BeforeQuestionSelected>
    {
        [SerializeField]
        private GameObject questionParentUI;
        [SerializeField]
        private Text questionText;
        [SerializeField]
        private Button[] answersButtons;
        [SerializeField]
        private Color rightAnswerColor;
        [SerializeField]
        private Color wrongAnswerColor;
        [SerializeField]
        private float flashDelay = 0.5f;
        [SerializeField]
        private int flashRepeats = 8;

        private ColorBlock defaultColorBlock;

        void Start()
        {
            defaultColorBlock = answersButtons[0].colors;
            //disable on start
            Services.Instance.EventAggregator.Subscribe<BeforeQuestionSelected>(this);
        }

        void OnDestroy()
        {
            Services.Instance.EventAggregator.Unsubscribe<BeforeQuestionSelected>(this);
        }

        public void Handler(object sender, BeforeQuestionSelected e)
        {
            ResetButtons();

            List<int> indexes = new List<int>();
            for (int i = 0; i < Settings.ANSWERS_COUNT; i++)
                indexes.Add(i);

            foreach (var ans in e.QnA.WrongAnswers)
            {
                var r = Random.Range(0, indexes.Count);
                var index = indexes[r];
                indexes.RemoveAt(r);

                answersButtons[index].GetComponentInChildren<Text>().text = ans;
                var tempIndexA = index;
                answersButtons[index].onClick.AddListener(() => OnAnswerPicked(false, tempIndexA));
            }

            answersButtons[indexes[0]].GetComponentInChildren<Text>().text = e.QnA.RightAnswer;
            var tempIndexB = indexes[0];
            answersButtons[indexes[0]].onClick.AddListener(() => OnAnswerPicked(true, tempIndexB));
        }


        void OnAnswerPicked(bool isRight, int  buttonIndex)
        {
            var color = isRight ? rightAnswerColor : wrongAnswerColor;
            var coccurentHandler = new Unity.Utils.CocurrentRoutineHandler(Disable,true, Utils.ButtonFlash(answersButtons[buttonIndex],defaultColorBlock, color, flashDelay, flashRepeats));

            Services.Instance.EventAggregator.Invoke(this,new AnswerPickedCoccurentEventArgs(isRight, coccurentHandler));
            coccurentHandler.Start();
        }

        void Disable()
        { 
            //set Active False
            throw new System.NotImplementedException();
        }

        void ResetButtons()
        {
            foreach (var button in answersButtons)
            {
                button.onClick.RemoveAllListeners();
                button.colors = defaultColorBlock;
            }
        }

        
    }
}
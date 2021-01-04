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
        private float falshDelay = 0.5f;
        [SerializeField]
        private int flashRepeats = 8;

        private ColorBlock defaultColorBlock;

        void Start()
        {
            defaultColorBlock = answersButtons[0].colors;
            //disable on start
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
                answersButtons[index].onClick.AddListener(() => OnAnswerPicked(false, answersButtons[tempIndexA]));
            }

            answersButtons[indexes[0]].GetComponentInChildren<Text>().text = e.QnA.RightAnswer;
            var tempIndexB = indexes[0];
            answersButtons[indexes[0]].onClick.AddListener(() => OnAnswerPicked(true, answersButtons[tempIndexB]));
        }


        void OnAnswerPicked(bool isRight, Button button)
        {
            Services.Instance.EventAggregator.Invoke(this, new BeforeAnswerPickedEventArgs(isRight));

            var sequence = new Unity.Utils.SequenceLoader(Disable);
            var color = isRight ? rightAnswerColor : wrongAnswerColor;
            sequence.AddEnumerator(ButtonFlash(button, color, falshDelay, flashRepeats));

            Services.Instance.EventAggregator.Invoke(this, new AnswerPickedSequence(isRight, sequence));
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

        IEnumerator ButtonFlash(Button button, Color color, float delay, int repeats)
        {
            ColorBlock colorBlock = defaultColorBlock;
            var defaultColor = colorBlock.normalColor;
            for (int i = 0; i < repeats; i++)
            {
                var t = i % 2 == 1 ? delay : 0f;
                do
                {
                    t = i % 2 == 1 ? t - Time.deltaTime : t + Time.deltaTime;
                    var targetColor = Vector4.Lerp(defaultColor, color, t / delay);
                    colorBlock.normalColor = targetColor;

                    button.colors = colorBlock;

                    yield return null;
                } while (t <= delay && t >= 0);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PizzaPie.QuestionsGame.Questions;
using PizzaPie.QuestionsGame.Events;
using PizzaPie.QuestionsGame.Wheel;

namespace PizzaPie.QuestionsGame.UI
{
    public class QuestionUIManager : MonoBehaviour, ISubscriber<Wheel.WheelStopSpinEventArgs>
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
        [SerializeField]
        private AudioClip isRightClip;
        [SerializeField]
        private AudioClip isWrongClip;

        private ColorBlock defaultColorBlock;

        void Start()
        {
            defaultColorBlock = answersButtons[0].colors;
            questionParentUI.SetActive(false);
            //disable on start
            Services.Instance.EventAggregator.Subscribe<WheelStopSpinEventArgs>(this);
        }

        public void Handler(object sender, WheelStopSpinEventArgs e)
        {
            e.CocurrentRoutine.AddOnFinishCallback(OnEnter);

            ResetButtons();
            List<int> indexes = new List<int>();
            for (int i = 0; i < Settings.ANSWERS_COUNT; i++)
                indexes.Add(i);

            var QnA = Services.Instance.QuestionsProvider.GetNextQuestion(e.QuestionCategory);
            questionText.text = QnA.Question;
            foreach (var ans in QnA.WrongAnswers)
            {
                var r = Random.Range(0, indexes.Count);
                var index = indexes[r];
                indexes.RemoveAt(r);

                answersButtons[index].GetComponentInChildren<Text>().text = ans;
                var tempIndexA = index;
                answersButtons[index].onClick.AddListener(() => OnAnswerPicked(false, tempIndexA));
            }

            answersButtons[indexes[0]].GetComponentInChildren<Text>().text = QnA.RightAnswer;
            var tempIndexB = indexes[0];
            answersButtons[indexes[0]].onClick.AddListener(() => OnAnswerPicked(true, tempIndexB));
        }

        private void OnEnter()
        {
            questionParentUI.SetActive(true);
        }

        private void OnAnswerPicked(bool isRight, int  buttonIndex)
        {
            var color = isRight ? rightAnswerColor : wrongAnswerColor;

            Services.Instance.SoundService.PlayClip(isRight ? isRightClip : isWrongClip, QuestionsGame.Sound.AudioType.SOUND_FX);

            var coccurentHandler = 
                new Unity.Utils.CocurrentRoutineHandler(OnExit, true,
                new System.Func<IEnumerator>(() => Utils.ButtonFlash(answersButtons[buttonIndex], defaultColorBlock, color, flashDelay, flashRepeats)));

            Services.Instance.EventAggregator.Invoke(this, new AnswerPickedCoccurentEventArgs(isRight, coccurentHandler));
            coccurentHandler.Start();
        }

        private void OnExit()
        {
            questionParentUI.SetActive(false);
        }

        private void ResetButtons()
        {
            foreach (var button in answersButtons)
            {
                button.onClick.RemoveAllListeners();
                button.colors = defaultColorBlock;
            }
        }

       
    }
}
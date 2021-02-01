using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PizzaPie.QuestionsGame.Questions;
using PizzaPie.QuestionsGame.Events;
using PizzaPie.QuestionsGame.Wheel;
using PizzaPie.QuestionsGame.States;

namespace PizzaPie.QuestionsGame.UI
{
    public class QuestionUIManager : MonoBehaviour, States.IState 
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
        private StateMachine stateMachine;
        private bool isEnd;

        public StateType GetStateType => StateType.QUESTION;
        public void Init(StateMachine stateMachine)
        {
            questionParentUI.SetActive(false);
            this.stateMachine = stateMachine;
            defaultColorBlock = answersButtons[0].colors;
        }

        public void Enter()
        {
            isEnd = false;
            questionParentUI.SetActive(true);

            ResetButtons();

            List<int> indexes = new List<int>();
            for (int i = 0; i < Settings.ANSWERS_COUNT; i++)
                indexes.Add(i);

            var QnA = Services.Instance.QuestionsProvider.GetNextQuestion(stateMachine.GetBlackBoardValue<CategoryDefinition>().Category);
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

        public void Exit()
        {
            questionParentUI.SetActive(false);
        }

        public void _Reset()
        {
        }

        public void Interupt()
        {
        }

        private void OnAnswerPicked(bool isRight, int  buttonIndex)
        {
            var color = isRight ? rightAnswerColor : wrongAnswerColor;
            Services.Instance.SoundService.PlayClip(isRight ? isRightClip : isWrongClip, QuestionsGame.Sound.AudioType.SOUND_FX);
            SetButtonsInteractable(false);

            bool isEnd = GameManager.Instance.IsEnd(isRight);
            var coccurentHandler = new Unity.Utils.CocurrentRoutineHandler();
            coccurentHandler.AddOnFinishCallback(()=>Transition(isEnd,isRight));
            coccurentHandler.AddRoutine(() => Utils.ButtonFlash(answersButtons[buttonIndex], defaultColorBlock, color, flashDelay, flashRepeats));
            coccurentHandler.Start();
        }

        private void Transition(bool isEnd,bool isRight)
        {
            if (isEnd)
            {
                stateMachine.SetBlackboardValue<AnswerPickedEventArgs>(new AnswerPickedEventArgs(isRight));
                stateMachine.ChangeState(StateType.END_GAME);
            }
            else
                stateMachine.ChangeState(StateType.WHEEL);
        }

        private void SetButtonsInteractable(bool isInteractable)
        {
            foreach (var button in answersButtons)
                button.interactable = isInteractable;
        }

        private void ResetButtons()
        {
            foreach (var button in answersButtons)
            {
                button.onClick.RemoveAllListeners();
                button.colors = defaultColorBlock;
            }
            SetButtonsInteractable(true);
        }

    }
}
using PizzaPie.QuestionsGame.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PizzaPie.QuestionsGame.UI
{
    public class EndGameUIManager : MonoBehaviour ,States.IState
    {
        [SerializeField]
        private GameObject endGameUIParent;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private string winText;
        [SerializeField]
        private string loseText;
        [SerializeField]
        private Text endGameText;
        [SerializeField]
        private Button playAgainButton;
        [SerializeField]
        private float fadeDelay;
        [SerializeField]
        private AudioClip winClip;
        [SerializeField]
        private AudioClip loseClip;
        [SerializeField]
        private AudioClip playAgainClip;

        public StateType GetStateType => StateType.END_GAME;
        private StateMachine stateMachine;

        public void Init(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            endGameUIParent.SetActive(false);
            playAgainButton.onClick.AddListener(OnPlayAgain);
        }

        public void Enter()
        {
            canvasGroup.alpha = 0f;
            endGameUIParent.SetActive(true);
            bool isWin = stateMachine.GetBlackBoardValue<PizzaPie.QuestionsGame.Questions.AnswerPickedEventArgs>().IsRight;
            Services.Instance.SoundService.PlayClip(isWin ? winClip : loseClip, QuestionsGame.Sound.AudioType.SOUND_FX);

            endGameText.text = isWin ? winText : loseText;
            StartCoroutine(Utils.CanvasGroupFade(canvasGroup, fadeDelay, 1));
        }

        public void Exit()
        {
            endGameUIParent.SetActive(false);
        }

        public void _Reset()
        {
        }

        public void Interupt()
        {
        }

        private void OnPlayAgain()
        {
            Services.Instance.SoundService.PlayClip(playAgainClip, Sound.AudioType.SOUND_FX);
            Services.Instance.EventAggregator.Invoke<PlayAgainEventArgs>(this, new PlayAgainEventArgs());
            stateMachine.ChangeState(StateType.DIFFICULTY);
            endGameUIParent.SetActive(false);
        }

    }
}
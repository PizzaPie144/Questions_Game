using PizzaPie.QuestionsGame.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PizzaPie.QuestionsGame.UI
{
    public class StartGameUIManager : MonoBehaviour , IState
    {
        [SerializeField]
        private GameObject parentUI;
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private AudioClip musicClip;
        [SerializeField]
        private AudioClip playButtonClip;

        private StateMachine stateMachine;

        public StateType GetStateType => StateType.GAME_START;

        private void OnPlayButton()
        {
            Services.Instance.SoundService.PlayClip(musicClip, Sound.AudioType.MUSIC);
            Services.Instance.SoundService.PlayClip(playButtonClip, Sound.AudioType.SOUND_FX);

            stateMachine.ChangeState(StateType.DIFFICULTY);
            //Services.Instance.EventAggregator.Invoke(this, new PlayButtonEventArgs());
        }

        public void Init(StateMachine stateMachine)
        {
            parentUI.SetActive(false);
            playButton.onClick.AddListener(OnPlayButton);
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            parentUI.SetActive(true);
        }

        public void Exit()
        {
            parentUI.SetActive(false);
        }
        public void _Reset()
        {

        }

        public void Interupt()
        {
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PizzaPie.QuestionsGame.UI
{
    public class StartGameUIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject parentUI;
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private AudioClip musicClip;
        [SerializeField]
        private AudioClip playButtonClip;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayButton);
            parentUI.SetActive(true);
        }

        private void OnPlayButton()
        {
            Services.Instance.SoundService.PlayClip(musicClip, QuestionsGame.Sound.AudioType.MUSIC);
            Services.Instance.SoundService.PlayClip(playButtonClip, QuestionsGame.Sound.AudioType.SOUND_FX);

            Services.Instance.EventAggregator.Invoke(this, new PlayButtonEventArgs());
            parentUI.SetActive(false);
        }
    }
}

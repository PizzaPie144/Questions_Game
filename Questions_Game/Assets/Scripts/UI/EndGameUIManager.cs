using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PizzaPie.QuestionsGame.UI
{
    public class EndGameUIManager : MonoBehaviour ,Events.ISubscriber<EndGameCocurrentEventArgs>
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

        private void Start()
        {
            endGameUIParent.SetActive(false);
            playAgainButton.onClick.AddListener(OnPlayAgain);
            Services.Instance.EventAggregator.Subscribe<EndGameCocurrentEventArgs>(this);
        }

        private void OnPlayAgain()
        {
            Services.Instance.SoundService.PlayClip(playAgainClip, QuestionsGame.Sound.AudioType.SOUND_FX);
            _Reset();
            Services.Instance.EventAggregator.Invoke<PlayAgainEventArgs>(this, new PlayAgainEventArgs());
            endGameUIParent.SetActive(false);
        }

        public void Handler(object sender, EndGameCocurrentEventArgs e)
        {
            endGameUIParent.SetActive(true);
            Services.Instance.SoundService.PlayClip(e.IsWin ? winClip : loseClip, QuestionsGame.Sound.AudioType.SOUND_FX);

            endGameText.text = e.IsWin ? winText : loseText;
            e.CocurrentRoutine.AddRoutine(FadeInRoutine);
        }

        private IEnumerator FadeInRoutine()
        {
            yield return StartCoroutine(Utils.CanvasGroupFade(canvasGroup, fadeDelay, 1));
        }

        private void _Reset()
        {
            endGameUIParent.SetActive(false);
            canvasGroup.alpha = 0;
        }
    }
}
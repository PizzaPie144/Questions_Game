using PizzaPie.Wheel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PizzaPie.UI
{
    public class WheelUIManager : MonoBehaviour , Events.ISubscriber<GameStartsEventArgs> ,Events.ISubscriber<WheelStopSpinEventArgs>
    {
        [SerializeField]
        private GameObject parentWindow;
        [SerializeField]
        private Button stopWheelButton;
        [SerializeField]
        private Image categoryImg;
        [SerializeField]
        private float initDelay;
        [SerializeField]
        private float categoryImgFadeDelay;
        [SerializeField]
        private float categoryImgStayDuration;
        [SerializeField]
        private CanvasGroup canvasGroup;
       
        private void Start()
        {
            _Reset();
            stopWheelButton.onClick.AddListener(OnStopButton);
            parentWindow.SetActive(false);

            Services.Instance.EventAggregator.Subscribe<GameStartsEventArgs>(this);
            Services.Instance.EventAggregator.Subscribe<WheelStopSpinEventArgs>(this);
        }

        public void Handler(object sender, GameStartsEventArgs e)
        {
            parentWindow.SetActive(true);
            StartCoroutine(InitRoutine());
        }

        public void Handler(object sender, WheelStopSpinEventArgs e)
        {
            categoryImg.sprite = Services.Instance.QuestionsProvider.GetCategoryDefinition(e.QuestionCategory).Sprite;
            e.CocurrentRoutine.AddRoutine(new System.Func<IEnumerator>(()=>CategoryImageFadeIn()));
            e.CocurrentRoutine.AddOnFinishCallback(OnExit);
        }

        private void OnExit()
        {
            _Reset();
            parentWindow.SetActive(false);
        }

        private void _Reset()
        {
            categoryImg.GetComponent<CanvasGroup>().alpha = 0;
            canvasGroup.alpha = 0;
        }

        private IEnumerator InitRoutine()
        {
            yield return StartCoroutine(Utils.CanvasGroupFade(canvasGroup, initDelay, 1));
            stopWheelButton.interactable = true;
        }

        private void OnStopButton()
        {
            Services.Instance.EventAggregator.Invoke(this, new Wheel.StopWheelButtonEventArgs());
            stopWheelButton.interactable = false;
        }

        private IEnumerator CategoryImageFadeIn()
        {
            yield return StartCoroutine(Utils.CanvasGroupFade(categoryImg.GetComponent<CanvasGroup>(),categoryImgFadeDelay, 1));
            yield return new WaitForSeconds(categoryImgStayDuration);
        }

        
    }
}
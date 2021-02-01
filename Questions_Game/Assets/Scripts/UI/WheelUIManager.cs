using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using PizzaPie.QuestionsGame.Wheel;
using PizzaPie.QuestionsGame.States;

namespace PizzaPie.QuestionsGame.UI
{
    public class WheelUIManager : MonoBehaviour , IState, Events.ISubscriber<WheelStopSpinEventArgs>
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

        public StateType GetStateType => StateType.WHEEL;

        private StateMachine stateMachine;

        public void Init(StateMachine stateMachine)
        {
            parentWindow.SetActive(false);
            this.stateMachine = stateMachine;
            stopWheelButton.onClick.AddListener(OnStopButton);
            Services.Instance.EventAggregator.Subscribe<WheelStopSpinEventArgs>(this);
        }

        public void Enter()
        {
            categoryImg.GetComponent<CanvasGroup>().alpha = 0;
            canvasGroup.alpha = 0;

            parentWindow.SetActive(true);
            StartCoroutine(InitRoutine());
        }

        public void Exit()
        {
            parentWindow.SetActive(false);

        }

        public void _Reset()
        {
        }

        public void Interupt()
        {
        }

        public void Handler(object sender, WheelStopSpinEventArgs e)
        {
            var categoryDefinition = Services.Instance.QuestionsProvider.GetCategoryDefinition(e.QuestionCategory);
            categoryImg.sprite = categoryDefinition.Sprite;
            e.CocurrentRoutine.AddRoutine(new System.Func<IEnumerator>(()=>CategoryImageFadeIn()));
            
            stateMachine.SetBlackboardValue<Questions.CategoryDefinition>(categoryDefinition);
            e.CocurrentRoutine.AddOnFinishCallback(()=>stateMachine.ChangeState(StateType.QUESTION));
        }

        private IEnumerator InitRoutine()
        {
            stopWheelButton.interactable = false;
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
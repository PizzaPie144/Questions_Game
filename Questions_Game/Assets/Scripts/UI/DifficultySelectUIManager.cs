using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PizzaPie.QuestionsGame.Events;
using PizzaPie.QuestionsGame.States;

namespace PizzaPie.QuestionsGame.UI
{
    public class DifficultySelectUIManager : MonoBehaviour ,IState
    {
        [SerializeField]
        private GameObject difficultySelectParentPanel;
        [SerializeField]
        private GameObject difficultySelectButtonPrefab;
        [SerializeField]
        private Color flashColor;
        [SerializeField]
        private float flashDelay;
        [SerializeField]
        private int flashRepeats;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float fadeInDelay;
        [SerializeField]
        private AudioClip clipOnSelected;

        private Button[] difficultySelectButtons;
        private DifficultyDefinition[] definitions;
        private StateMachine stateMachine;
        private ColorBlock defaultColorBlock;
        public StateType GetStateType => StateType.DIFFICULTY;


        public void Init(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            difficultySelectParentPanel.SetActive(false);

            definitions = Resources.LoadAll<DifficultyDefinition>("Difficulty Definitions");      //would make more sense to have them delivered from elsewhere
            System.Array.Sort(definitions);
            difficultySelectButtons = new Button[definitions.Length];

            for (int i = 0; i < definitions.Length; i++)
            {
                var go = Instantiate(difficultySelectButtonPrefab, difficultySelectParentPanel.transform);
                difficultySelectButtons[i] = go.GetComponent<Button>();
                go.GetComponentInChildren<Text>().text = definitions[i].DisplayName;
                var index = i;
                go.GetComponent<Button>().onClick.AddListener(() => OnDifficultySelected(index));
            }
            defaultColorBlock = difficultySelectButtons[0].colors;
        }

        public void Enter()
        {
            canvasGroup.alpha = 0f;
            foreach (var button in difficultySelectButtons)
                button.colors = defaultColorBlock;

            difficultySelectParentPanel.SetActive(true);
            SetButtonsInteractable(difficultySelectButtons, false);

            Unity.Utils.CocurrentRoutineHandler cocurrentRoutineHandler = new Unity.Utils.CocurrentRoutineHandler();
            cocurrentRoutineHandler.AddRoutine(() => Utils.CanvasGroupFade(canvasGroup, fadeInDelay, 1f));
            cocurrentRoutineHandler.AddOnFinishCallback(() => SetButtonsInteractable(difficultySelectButtons, true));
            cocurrentRoutineHandler.Start();
        }

        public void Exit()
        {
            difficultySelectParentPanel.SetActive(false);
        }

        public void _Reset()
        {
        }

        public void Interupt()
        {
        }

        private void OnDifficultySelected(int index)
        {
            Services.Instance.SoundService.PlayClip(clipOnSelected, Sound.AudioType.SOUND_FX);
            stateMachine.SetBlackboardValue<DifficultyDefinition>(definitions[index]);

            var coccurrentRoutine = new Unity.Utils.CocurrentRoutineHandler();
            coccurrentRoutine.AddRoutine(
                ()=> Utils.ButtonFlash(difficultySelectButtons[index], difficultySelectButtons[index].colors, flashColor, flashDelay, flashRepeats));

            Services.Instance.EventAggregator.Invoke(this, new DifficultySelectedEventArgs(definitions[index], coccurrentRoutine));
            coccurrentRoutine.AddOnFinishCallback(()=> stateMachine.ChangeState(StateType.LOADING));

            coccurrentRoutine.Start();
        }

        private void SetButtonsInteractable(Button[] buttons, bool isInteractable)
        {
            foreach (var button in buttons)
                button.interactable = isInteractable;
        }
    }
}


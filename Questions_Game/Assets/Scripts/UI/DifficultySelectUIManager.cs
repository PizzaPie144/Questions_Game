using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PizzaPie.Events;

namespace PizzaPie.UI
{
    public class DifficultySelectUIManager : MonoBehaviour , ISubscriber<PlayButtonEventArgs>
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

        private Button[] difficultySelectButtons;
        private DifficultyDefinition[] definitions;

        private void Start()
        {
            Services.Instance.EventAggregator.Subscribe<PlayButtonEventArgs>(this);
            difficultySelectParentPanel.SetActive(false);
        }


        private void Init()
        {
            definitions = Resources.LoadAll<DifficultyDefinition>("Difficulty Definitions");      //would make more sense to have them delivered from elsewhere
            System.Array.Sort(definitions);
            difficultySelectButtons = new Button[definitions.Length];
            difficultySelectParentPanel.SetActive(true);

            for (int i = 0; i < definitions.Length; i++)
            {
                var go = Instantiate(difficultySelectButtonPrefab,difficultySelectParentPanel.transform);
                difficultySelectButtons[i] = go.GetComponent<Button>();
                go.GetComponentInChildren<Text>().text = definitions[i].DisplayName;
                var index = i;
                go.GetComponent<Button>().onClick.AddListener(() => OnDifficultySelected(index));

            }
        }

        private void OnDifficultySelected(int index)
        {

            var coccurrentRoutine = new Unity.Utils.CocurrentRoutineHandler(Disable, true,
                new System.Func<IEnumerator>(() => Utils.ButtonFlash(difficultySelectButtons[index],difficultySelectButtons[index].colors,flashColor,flashDelay,flashRepeats)));

            Services.Instance.EventAggregator.Invoke(this, new DifficultySelectedEventArgs(definitions[index], coccurrentRoutine));
            coccurrentRoutine.Start();
        }

        public void _Reset()
        {
            canvasGroup.alpha = 0f;
            foreach (var button in difficultySelectButtons)
                Destroy(button.gameObject);
        }


        private void Disable()
        {
            difficultySelectParentPanel.SetActive(false);
            _Reset();
        }

        public void Handler(object sender, PlayButtonEventArgs e)
        {
            canvasGroup.alpha = 0f;
            Init();
            difficultySelectParentPanel.SetActive(true);
            StartCoroutine(Utils.CanvasGroupFade(canvasGroup, fadeInDelay, 1f));
        }
    }
}


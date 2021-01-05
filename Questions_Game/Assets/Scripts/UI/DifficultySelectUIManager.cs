using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PizzaPie.Events;

namespace PizzaPie.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DifficultySelectUIManager : MonoBehaviour , ISubscriber<PlayButtonEventArgs>
    {
        [SerializeField]
        private GameObject difficultySelectPanel;
        [SerializeField]
        private GameObject difficultySelectButtonPrefab;
        [SerializeField]
        private Color flashColor;
        [SerializeField]
        private float flashDelay;
        [SerializeField]
        private int flashRepeats;

        private CanvasGroup canvasGroup;

        private Button[] difficultySelectButtons;
        private DifficultyDefinition[] definitions;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            Services.Instance.EventAggregator.Subscribe<PlayButtonEventArgs>(this);
        }

        private void OnDestroy()
        {
            Services.Instance.EventAggregator.Unsubscribe<PlayButtonEventArgs>(this);
        }

        private void Init()
        {
            @Reset();
            definitions = Resources.LoadAll<DifficultyDefinition>("Resources/Difficulty Definitions");
            System.Array.Sort(definitions);

            for (int i = 0; i < definitions.Length; i++)
            {
                var go = Instantiate(difficultySelectButtonPrefab);

                difficultySelectButtons[i] = go.GetComponent<Button>();
                go.GetComponentInChildren<Text>().text = definitions[i].DisplayName;
                var index = i;
                go.GetComponent<Button>().onClick.AddListener(() => OnDifficultySelected(index));
            }
        }

        private void OnDifficultySelected(int index)
        {
            var coccurrentRoutine = new Unity.Utils.CocurrentRoutineHandler(Disable, true, 
                Utils.ButtonFlash(difficultySelectButtons[index],difficultySelectButtons[index].colors,flashColor,flashDelay,flashRepeats));

            Services.Instance.EventAggregator.Invoke(this, new DifficultySelectedEventArgs(definitions[index], coccurrentRoutine));
            coccurrentRoutine.Start();
        }

        public void @Reset()
        {
            //useless as destroy runs on the end of the frame
            canvasGroup.alpha = 1f;
            for (int i = difficultySelectPanel.transform.childCount - 1; i >= 0; i--)
                Destroy(difficultySelectPanel.transform.GetChild(i));
        }


        private void Disable()
        {
            difficultySelectPanel.SetActive(false);
        }

        public void Handler(object sender, PlayButtonEventArgs e)
        {
            Init();
            canvasGroup.alpha = 0f;
            StartCoroutine(Utils.UIFade(canvasGroup, 3f, 1f));
        }
    }
}

/*
 * On Init Load After difficulty Select
 * Add a routine that takes Time.time and checks if 5 secs passed (so to prevent loading screen flash)
 * Obvisously nto here
 * 
 * Category definitions to new provider
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PizzaPie.UI
{
    public class StartGameUIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject parentUI;
        [SerializeField]
        private Button playButton;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayButton);
            parentUI.SetActive(true);
        }

        private void OnPlayButton()
        {
            Services.Instance.EventAggregator.Invoke(this, new PlayButtonEventArgs());
            parentUI.SetActive(false);
        }
    }
}

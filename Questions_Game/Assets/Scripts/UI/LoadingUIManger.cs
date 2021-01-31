using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.UI
{
    public class LoadingUIManger : MonoBehaviour , Events.ISubscriber<LoadEventArgs>
    {
        [SerializeField]
        private GameObject parentUI;

        private void Start()
        {
            Services.Instance.EventAggregator.Subscribe<LoadEventArgs>(this);
            parentUI.SetActive(false);
        }

        public void Handler(object sender, LoadEventArgs e)
        {
            parentUI.SetActive(true);
            e.SequenceLoader.AddOnFinishAction(() => parentUI.SetActive(false));
        }
    }
}

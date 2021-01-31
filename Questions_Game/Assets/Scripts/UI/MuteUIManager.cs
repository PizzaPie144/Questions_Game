using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PizzaPie.QuestionsGame.UI
{
    public class MuteUIManager : MonoBehaviour
    {
        bool isMute;
        Image img;

        [SerializeField]
        private Sprite muteSprite;
        [SerializeField]
        private Sprite unmuteSprite;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnMuteButton);
            img = GetComponent<Image>();
            img.sprite = muteSprite;
        }

        void OnMuteButton()
        {
            isMute = !isMute;
            img.sprite = isMute ? unmuteSprite : muteSprite;
            Services.Instance.SoundService.Mute(isMute);
        }
    }
}

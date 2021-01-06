using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.Sound
{
    public interface ISoundService
    {
        void PlayClip(AudioClip clip, AudioType type);
        void Mute(bool isMute);
    }

    public enum AudioType
    {
        MUSIC,
        SOUND_FX
    }
}
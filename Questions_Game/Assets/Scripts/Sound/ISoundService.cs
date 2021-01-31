using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.Sound
{
    public interface ISoundService
    {
        void PlayClip(AudioClip clip, AudioType type, bool loop = false);
        void Mute(bool isMute);

        void Stop(AudioType type);
    }

    public enum AudioType
    {
        MUSIC,
        SOUND_FX
    }
}
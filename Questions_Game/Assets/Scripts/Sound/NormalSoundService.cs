using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.Sound
{
    public class NormalSoundService : ISoundService
    {
        private AudioSource sourceMusic;
        private AudioSource sourceFX;
        private GameObject host;

        public NormalSoundService()
        {
            host = new GameObject("Audio Source");
            sourceMusic = host.AddComponent<AudioSource>();
            sourceFX = host.AddComponent<AudioSource>();

            sourceMusic.loop = true;
            sourceFX.loop = false;
            sourceMusic.playOnAwake = false;
            sourceFX.playOnAwake = false;
        }

        public void Mute(bool isMute)
        {
            var volum = isMute ? 0 : 1;
            sourceMusic.volume = volum;
            sourceFX.volume = volum;
        }

        public void PlayClip(AudioClip clip, AudioType type, bool loop = false)
        {
            switch (type)
            {
                case AudioType.SOUND_FX:
                    sourceFX.clip = clip;
                    sourceFX.Play();
                    sourceFX.loop = false;
                    break;
                case AudioType.MUSIC:
                    sourceMusic.clip = clip;
                    sourceMusic.Play();
                    break;
            }
        }

        public void Stop(AudioType type)
        {
            switch (type)
            {
                case AudioType.SOUND_FX:
                    sourceFX.Stop();
                    break;
                case AudioType.MUSIC:
                    sourceMusic.Stop();
                    break;
            }
        }

    }

}
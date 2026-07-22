using System;
using UnityEngine;

namespace Project.Sound
{
    [Serializable]
    public class SoundSources
    {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        
        public AudioSource SfxSource => sfxSource;
        public AudioSource MusicSource => musicSource;
    }
}
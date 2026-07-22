using UnityEngine;

namespace Project.Sound
{
    internal class SoundSaveLoad
    {
        private const string MUSIC_KEY = "FemnusJam_MusicVolume";
        private const string SFX_KEY = "FremnusJam_SFXVolume";

        public float GetSavedMusicVolume() => PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        public float GetSavedSfxVolume() => PlayerPrefs.GetFloat(SFX_KEY, 1f);
        
        public void SaveMusicVolume(float volume) => PlayerPrefs.SetFloat(MUSIC_KEY, volume);
        public void SaveSfxVolume(float volume) => PlayerPrefs.SetFloat(SFX_KEY, volume);
    }
}
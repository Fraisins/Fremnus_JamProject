using System;
using UnityEngine;

namespace Project.Sound
{
    public class SoundService : ISoundService
    {
        // dependencies
        private readonly AudioSource _sfxSource;
        private readonly AudioSource _musicSource;
        
        // child classes
        private readonly SoundSaveLoad _soundSaveLoad = new();
        
        // public
        public float MusicVolume => _musicSource.volume;
        public float SfxVolume => _sfxSource.volume;
        
        public SoundService(SoundSources sources)
        {
            _sfxSource = sources.SfxSource;
            _musicSource = sources.MusicSource;
            _musicSource.loop = true;
            
            TryLoadSavedVolume();
        }

        #region Player
        
        public void PlaySfx(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (_musicSource.clip == clip && _musicSource.isPlaying) return;
            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }
        
        public void StopMusic() => _musicSource.Stop();

        #endregion
        
        #region Volume

        public void SetMusicVolume(float value)
        {
            _musicSource.volume = value;
            _soundSaveLoad.SaveMusicVolume(_musicSource.volume);
        }

        public void SetSfxVolume(float value)
        {
            _sfxSource.volume = value;
            _soundSaveLoad.SaveSfxVolume(_sfxSource.volume);
        }

        private void TryLoadSavedVolume()
        {
            SetMusicVolume(_soundSaveLoad.GetSavedMusicVolume());
            SetSfxVolume(_soundSaveLoad.GetSavedSfxVolume());
        }
        
        #endregion
    }
}
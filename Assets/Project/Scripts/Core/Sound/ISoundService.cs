using System;
using UnityEngine;

namespace Project.Sound
{
    public interface ISoundService
    {
        #region Player
        
        public void PlaySfx(AudioClip clip);
        public void PlayMusic(AudioClip clip, bool loop = true);
        public void StopMusic();
        
        #endregion
        
        #region Volume

        public float MusicVolume { get; }
        public float SfxVolume { get; }
        
        public void SetMusicVolume(float value);
        public void SetSfxVolume(float value);
        
        #endregion
    }
}
using Project.Sound;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.UI.SoundMenu
{
    public class VolumeController : MonoBehaviour
    { 
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        
        private ISoundService _soundService;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
            musicVolumeSlider.SetValueWithoutNotify(soundService.MusicVolume);
            sfxVolumeSlider.SetValueWithoutNotify(soundService.SfxVolume);
        }

        public void Awake()
        {
            if (musicVolumeSlider == null)
                throw new System.NullReferenceException("[SoundMenuView] musicVolumeSlider is null");
            if (sfxVolumeSlider == null)
                throw new System.NullReferenceException("[SoundMenuView] sfxVolumeSlider is null");
            
            musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(HandleSfxVolumeChanged);
        }

        private void HandleMusicVolumeChanged(float value) => _soundService.SetMusicVolume(value);
        private void HandleSfxVolumeChanged(float value) => _soundService.SetSfxVolume(value);
    }
}
using Project.Sound;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    [RequireComponent(typeof(CustomButton))]
    public class CustomButtonSoundPlayer : MonoBehaviour
    {
        private ISoundService _soundService;
        private CustomButton _button;
        
        [Header("Sound")]
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip pressSound;

        [Inject]
        private void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        private void Awake()
        {
            _button = GetComponent<CustomButton>();
        }
        
        private void OnEnable()
        {
            _button.onHover.AddListener(OnHover);
            _button.onPressed.AddListener(OnPressed);
        }

        private void OnDisable()
        {
            _button.onHover.RemoveListener(OnHover);
            _button.onPressed.RemoveListener(OnPressed);
        }

        private void OnHover()
        {
            if (hoverSound == null) return;
            Play(hoverSound);
        }

        private void OnPressed()
        {
            if (pressSound == null) return;
            Play(pressSound);
        }
        
        private void Play(AudioClip clip)
        {
            if (_soundService == null)
            {
                Debug.LogWarning($"[CustomButtonSoundPlayer] Sound Service is null on {gameObject.name}.");
                return;
            }
            
            _soundService.PlaySfx(clip);
        }

        private void OnDestroy()
        {
            _button.onHover.RemoveListener(OnHover);
            _button.onPressed.RemoveListener(OnPressed);
        }
    }
}
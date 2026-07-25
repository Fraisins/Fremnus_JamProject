using PrimeTween;
using Project.Sound;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class GameUIMenu : MonoBehaviour
    {
        private ISoundService _soundService;
        public bool IsActive { get; protected set; }
        
        [Header("Appearance animation")]
        [SerializeField] protected float appearDuration = 0.25f;
        [SerializeField] protected float disappearDuration = 0.10f;
        [SerializeField] protected AudioClip appearClip;

        [Inject]
        private void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }
        
        private Tween _appearanceTween;
        private float _originalYScale;

        private void Awake()
        {
            _originalYScale = transform.localScale.y;
        }

        public virtual Tween PlayAppear()
        {
            if (_appearanceTween.isAlive)
                _appearanceTween.Stop();

            IsActive = true;
            PlayAppearSound();
            
            transform.localScale = new Vector3(
                transform.localScale.x,
                0f,
                transform.localScale.z);
            
            _appearanceTween = Tween.ScaleY(
                transform, 
                _originalYScale, 
                appearDuration,
                ease : Ease.OutBack,
                useUnscaledTime: true);
            
            return _appearanceTween;
        }

        public virtual Tween PlayHide()
        {
            if (_appearanceTween.isAlive)
                _appearanceTween.Stop();
            
            IsActive = false;
            PlayAppearSound();
            
            _appearanceTween = Tween.ScaleY(
                transform, 
                0f, 
                appearDuration,
                ease : Ease.InBack,
                useUnscaledTime: true);
            
            return _appearanceTween;
        }

        public void PlayAppearSound()
        {
            if (_soundService == null)
            {
                Debug.LogWarning("[GameUIMenu] Sound Service is null");
                return;
            }
            if (appearClip == null)
                return;
            
            _soundService.PlaySfx(appearClip);
        }

        private void OnDestroy()
        {
           if (_appearanceTween.isAlive)
               _appearanceTween.Stop();
        }
    }
}
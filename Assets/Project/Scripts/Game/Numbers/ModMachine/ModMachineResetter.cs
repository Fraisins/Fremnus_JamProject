using PrimeTween;
using Project.Core.Numbers;
using Project.Core.Robot;
using Project.Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Project.Game.Numbers
{
    public class ModMachineResetter : MonoBehaviour, IPointerClickHandler
    {
        private IModMachineService _modMachineService;
        private ISoundService _soundService;
        private IRobotService _robotService;

        [Header("Animation")]
        [SerializeField] private Transform leverTransform;
        [SerializeField] private float pullRotation = 90f;
        [SerializeField] private float pullDuration = 0.25f;
        
        [Header("Sound")]
        [SerializeField] private AudioClip pullSound;
        
        private Tween _pullTween;
        
        [Inject]
        public void Construct(IModMachineService modMachineService, ISoundService soundService,
            IRobotService robotService)
        {
            _modMachineService = modMachineService;
            _soundService = soundService;
            _robotService = robotService;

            _robotService.Request.OnRequestChanged += HandleRequestChanged;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_pullTween.isAlive) // pls don't pull multiple times
                return;
            
            _modMachineService.RecreateModificators();
            PlayLeverPullAnimation();
            PlayLeverPullSound();
        }

        private void PlayLeverPullAnimation()
        {
            _pullTween = Tween.LocalRotation(
                leverTransform, 
                startValue: Quaternion.identity,
                endValue: Quaternion.Euler(pullRotation, 0f, 0f), 
                duration: pullDuration, 
                cycles: 2, 
                cycleMode: CycleMode.Yoyo,
                ease: Ease.InOutQuad
            );
        }

        private void PlayLeverPullSound()
        {
            _soundService.PlaySfx(pullSound);
        }
        
        private void HandleRequestChanged()
        {
            if (_pullTween.isAlive) // pls don't pull multiple times
                return;
            
            _modMachineService.RecreateModificators();
            PlayLeverPullAnimation();
            PlayLeverPullSound();
        }

        private void OnDestroy()
        {
            if (_robotService?.Request != null)
                _robotService.Request.OnRequestChanged -= HandleRequestChanged;
            
            if (_pullTween.isAlive)
                _pullTween.Stop();
        }
    }
}
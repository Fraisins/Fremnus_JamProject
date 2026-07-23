using PrimeTween;
using Project.Core.Numbers;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Project.Game.Numbers
{
    public class ModMachineResetter : MonoBehaviour, IPointerClickHandler
    {
        IModMachineService _modMachineService;

        [SerializeField] private Transform leverTransform;
        [SerializeField] private float pullRotation = 90f;
        [SerializeField] private float pullDuration = 0.25f;
        private Tween _pullTween;
        
        [Inject]
        public void Construct(IModMachineService modMachineService)
        {
            _modMachineService = modMachineService;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_pullTween.isAlive) // pls don't pull multiple times
                return;
            
            _modMachineService.RecreateModificators();
            PlayLevelPullAnimation();
        }

        private void PlayLevelPullAnimation()
        {
            _pullTween = Tween.LocalRotation(
                leverTransform, 
                endValue: Quaternion.Euler(pullRotation, 0f, 0f), 
                duration: pullDuration, 
                cycles: 2, 
                cycleMode: CycleMode.Yoyo,
                ease: Ease.InOutQuad
            );
        }
    }
}
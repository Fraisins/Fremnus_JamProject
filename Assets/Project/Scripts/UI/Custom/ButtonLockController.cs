using Project.Core.Events;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    // Well, no time to think better solutions for this...
    [RequireComponent(typeof(CustomButton))]
    public class ButtonLockController : MonoBehaviour 
    {
        private IEventBus _eventBus;
        private CustomButton _button;
        [SerializeField] private bool lockWhenPaused = true;
        
        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            eventBus.Subscribe<GamePausedEvent>(HandlePausedEvent);
            eventBus.Subscribe<GameResumedEvent>(HandleResumedEvent);
        }

        private void Awake()
        {
            _button = GetComponent<CustomButton>();
        }
        
        private void HandlePausedEvent(GamePausedEvent obj)
        {
            if (lockWhenPaused)
                _button.SetLocked(true);
        }

        private void HandleResumedEvent(GameResumedEvent obj)
        {
            if (lockWhenPaused)
                _button.SetLocked(false);
        }

        private void OnDestroy()
        {
            if (_eventBus != null)
            {
                _eventBus.Unsubscribe<GamePausedEvent>(HandlePausedEvent);
                _eventBus.Unsubscribe<GameResumedEvent>(HandleResumedEvent);
            }
        }
    }
}
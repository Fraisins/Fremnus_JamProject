using System;
using Project.Core.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Core.Pause
{
    public class PauseService : IPauseService
    {
        private readonly IEventBus _eventBus;
        public bool IsPaused { get; private set; } = false;
        public bool CanBePaused { get; private set; } = true;
        public bool CanBeResumed { get; private set; } = true;
        
        public PauseService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public void Pause(bool showPauseMenu = true)
        {
            if (IsPaused || !CanBePaused) return;
            
            Debug.Log("[Pause Service] Game paused!");
            
            Time.timeScale = 0f;
            IsPaused = true;
            _eventBus.Publish(new GamePausedEvent(showPauseMenu));
        }

        public void Resume()
        {
            if (!IsPaused || !CanBeResumed) return;
            
            
            Debug.Log("[Pause Service] Game resumed!");
            
            Time.timeScale = 1f;
            IsPaused = false;
            _eventBus.Publish<GameResumedEvent>(default);
        }

        public void TogglePause()
        {
            if (!IsPaused) Pause();
            else Resume();
        }

        public void SetCanBePaused(bool canBePaused)
        {
            CanBePaused = canBePaused;
        }

        public void SetCanBeResumed(bool canBeResumed)
        {
            CanBeResumed = canBeResumed;
        }
    }
}
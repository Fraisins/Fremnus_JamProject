using System;
using Project.Core.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Core.Pause
{
    public class PauseService : IPauseService
    {
        private readonly IEventBus _eventBus;
        public bool IsPaused { get; private set; }
        
        public PauseService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public void Pause()
        {
            if (IsPaused) return;
            
            Time.timeScale = 0f;
            IsPaused = true;
            _eventBus.Publish<GamePausedEvent>(default);
        }

        public void Resume()
        {
            if (!IsPaused) return;
            
            Time.timeScale = 1f;
            IsPaused = false;
            _eventBus.Publish<GameResumedEvent>(default);
        }

        public void TogglePause()
        {
            if (!IsPaused) Pause();
            else Resume();
        }
    }
}
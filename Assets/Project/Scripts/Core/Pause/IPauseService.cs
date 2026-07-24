using System;

namespace Project.Core.Pause
{
    public interface IPauseService
    {
        public bool IsPaused { get; }
        public bool CanBePaused { get; }
        
        public void Pause();
        public void Resume();
        public void TogglePause();
        
        public void SetCanBePaused(bool canBePaused);
    }
}
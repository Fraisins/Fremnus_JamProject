using System;

namespace Project.Core.Pause
{
    public interface IPauseService
    {
        public bool IsPaused { get; }
        public bool CanBePaused { get; }
        public bool CanBeResumed { get; }
        
        public void Pause(bool showPauseMenu = true);
        public void Resume();
        public void TogglePause();
        
        public void SetCanBePaused(bool canBePaused);
        public void SetCanBeResumed(bool canBeResumed);
    }
}
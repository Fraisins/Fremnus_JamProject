using Project.Core.Pause;
using UnityEngine;

namespace Project.UI
{
    public class PauseMenuView : GameUIMenu
    {
        private IPauseService _pauseService;
        
        public void Init(IPauseService pauseService)
        {
            _pauseService = pauseService;
        }
                
        public void Resume()
        {
            _pauseService.Resume();
        }
    }
}
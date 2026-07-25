using Project.Core.Pause;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class PauseMenuView : GameUIMenu
    {
        private IPauseService _pauseService;
        
        [Inject]
        public void Construct(IPauseService pauseService)
        {
            _pauseService = pauseService;
        }
                
        public void Resume()
        {
            _pauseService.Resume();
        }
    }
}
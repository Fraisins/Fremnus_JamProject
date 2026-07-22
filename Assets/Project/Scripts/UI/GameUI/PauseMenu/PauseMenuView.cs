using Project.Core.Pause;
using UnityEngine;

namespace Project.UI
{
    public class PauseMenuView : MonoBehaviour
    {
        private IPauseService _pauseService;
        
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
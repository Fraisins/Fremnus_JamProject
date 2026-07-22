using Project.Core;
using Project.Core.Pause;
using Project.Scripts.UI.Data;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class GameUIView : MonoBehaviour
    {
        private ISceneService _sceneService;
        
        [SerializeField] private PauseMenuView pauseMenu;
        public PauseMenuView PauseMenu => pauseMenu;

        [Inject]
        public void Construct(PersistentUIContainer persistentUIContainer,
            ISceneService sceneService, IPauseService pauseService)
        {
            transform.SetParent(persistentUIContainer.Container); // haha...
            
            _sceneService = sceneService;
            pauseMenu.Construct(pauseService);
        }

        public void ReturnToMainMenu()
        {
            _sceneService.OpenMenu();
        }
    }
}
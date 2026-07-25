using System;
using PrimeTween;
using Project.Core;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class GameUIView : MonoBehaviour
    {
        private ISceneService _sceneService;
        
        [SerializeField] private PauseMenuView pauseMenu;
        [SerializeField] private GameOverMenuView gameOverMenu;
        [SerializeField] private LevelFinishedMenu levelFinishedMenu;
        
        public PauseMenuView PauseMenu => pauseMenu;
        public GameOverMenuView GameOverMenu => gameOverMenu;
        public LevelFinishedMenu LevelFinishedMenu => levelFinishedMenu;
        
        public event Action OnReturnToMainMenu;

        [Inject]
        public void Construct(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }

        public void ReturnToMainMenu()
        {
            OnReturnToMainMenu?.Invoke();
            _sceneService.OpenMenuFromGame();
        }
    }
}
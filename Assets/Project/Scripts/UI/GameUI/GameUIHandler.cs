using PrimeTween;
using Project.Core.Events;
using Project.Core.Game;
using Project.Core.Pause;
using UnityEngine;
using VContainer.Unity;

namespace Project.UI
{
    public class GameUIHandler : IInitializable, IStartable, System.IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly IPauseService _pauseService;
        
        private readonly GameUIView _gameUI;
        private readonly Transform _uiContainer;
        
        private readonly Tween _menuShowTween;

        public GameUIHandler(IEventBus eventBus, GameUIView gameUI, PersistentUIContainer persistentUIContainer, 
            IPauseService pauseService, IGameService gameService)
        {
            _eventBus = eventBus;
            _gameUI = gameUI;
            _uiContainer = persistentUIContainer.Container;
            _pauseService = pauseService;
            _gameUI.PauseMenu.Init(_pauseService);
            _gameUI.GameOverMenu.Init(gameService);
            _gameUI.LevelFinishedMenu.Init(gameService);
        }

        public void Initialize()
        {
            _eventBus.Subscribe<GamePausedEvent>(ShowPauseMenu);
            _eventBus.Subscribe<GameResumedEvent>(HidePauseMenu);
            
            _eventBus.Subscribe<GameOverEvent>(ShowGameOverMenu);
            _gameUI.GameOverMenu.OnHideRequested += HideGameOverMenu;
            
            _eventBus.Subscribe<GameLevelFinishedEvent>(ShowLevelFinishedMenu);
            _gameUI.LevelFinishedMenu.OnHideRequested += HideLevelFinishedMenu;
        }

        public void Start()
        {
            _gameUI.transform.SetParent(_uiContainer);
            _gameUI.gameObject.SetActive(true);
            
            _gameUI.PauseMenu.gameObject.SetActive(false);
            _gameUI.GameOverMenu.gameObject.SetActive(false);
            _gameUI.LevelFinishedMenu.gameObject.SetActive(false);
        }

        #region Pause Menu

        private void ShowPauseMenu(GamePausedEvent gamePausedEvent)
        {
            _gameUI.PauseMenu.gameObject.SetActive(true);
            _gameUI.PauseMenu.PlayAppear();
        }

        private void HidePauseMenu(GameResumedEvent gameResumedEvent)
        {
            _gameUI.PauseMenu.PlayHide().OnComplete(()
                => _gameUI.PauseMenu.gameObject.SetActive(false));
        }

        #endregion

        #region GameOver Menu

        private void ShowGameOverMenu(GameOverEvent gameOverEvent)
        {
            _pauseService.SetCanBePaused(false);
            
            _gameUI.GameOverMenu.gameObject.SetActive(true);
            _gameUI.GameOverMenu.PlayAppear();
        }

        private void HideGameOverMenu()
        {
            _pauseService.SetCanBePaused(true);
            
            _gameUI.GameOverMenu.PlayHide().OnComplete(() =>
                _gameUI.GameOverMenu.gameObject.SetActive(false));
        }

        #endregion
        
        #region LevelFinished Menu

        private void ShowLevelFinishedMenu(GameLevelFinishedEvent gameLevelFinishedEvent)
        {
            _pauseService.SetCanBePaused(false);
            
            _gameUI.LevelFinishedMenu.gameObject.SetActive(true);
            _gameUI.LevelFinishedMenu.PlayAppear();
        }

        private void HideLevelFinishedMenu()
        {
            _pauseService.SetCanBePaused(true);
            
            _gameUI.LevelFinishedMenu.PlayHide().OnComplete(() =>
                _gameUI.LevelFinishedMenu.gameObject.SetActive(false));
        }
        
        #endregion
        
        public void Dispose()
        {
            if (_gameUI == null) return;
            _gameUI.gameObject.SetActive(false);
            Object.Destroy(_gameUI.gameObject);
            
            _eventBus.Unsubscribe<GamePausedEvent>(ShowPauseMenu);
            _eventBus.Unsubscribe<GameResumedEvent>(HidePauseMenu);
            
            _eventBus.Unsubscribe<GameOverEvent>(ShowGameOverMenu);
            _gameUI.LevelFinishedMenu.OnHideRequested -= HideGameOverMenu;
            
            _eventBus.Unsubscribe<GameLevelFinishedEvent>(ShowLevelFinishedMenu);
            _gameUI.LevelFinishedMenu.OnHideRequested -= HideLevelFinishedMenu;
        }
    }
}
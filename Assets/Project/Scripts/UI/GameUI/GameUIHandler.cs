using Project.Core.Events;
using Project.Core.Game;
using Project.Core.Pause;
using UnityEngine;
using VContainer.Unity;

namespace Project.UI
{
    public class GameUIHandler : IStartable, System.IDisposable
    {
        private readonly GameUIView _gameUI;
        private readonly Transform _uiContainer;

        public GameUIHandler(IEventBus eventBus, GameUIView gameUI, PersistentUIContainer persistentUIContainer, 
            IPauseService pauseService, IGameService gameService)
        {
            _gameUI = gameUI;
            _uiContainer = persistentUIContainer.Container;
            _gameUI.PauseMenu.Init(pauseService);
            _gameUI.GameOverMenu.Init(eventBus, gameService);
            _gameUI.LevelFinishedMenu.Init(eventBus, gameService);
        }

        public void Start()
        {
            _gameUI.transform.SetParent(_uiContainer);
            _gameUI.gameObject.SetActive(true);
        }
        
        public void Dispose()
        {
            if (_gameUI == null) return;
            _gameUI.gameObject.SetActive(false);
            Object.Destroy(_gameUI.gameObject);
        }
    }
}
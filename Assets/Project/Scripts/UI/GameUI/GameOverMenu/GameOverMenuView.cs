using Project.Core.Events;
using Project.Core.Game;
using UnityEngine;

namespace Project.UI
{
    public class GameOverMenuView : MonoBehaviour
    {
        private IEventBus _eventBus;
        private IGameService _gameService;
        
        [SerializeField] private Transform visualTransform;

        public void Init(IEventBus eventBus, IGameService gameService)
        {
            _eventBus = eventBus;
            _gameService = gameService;
            
            _eventBus.Subscribe<GameOverEvent>(HandleGameOver);
        }

        private void Awake()
        {
            visualTransform.gameObject.SetActive(false);
        }

        private void HandleGameOver(GameOverEvent gameOverEvent)
        {
            visualTransform.gameObject.SetActive(true);
        }

        public void Retry()
        {
            _gameService.RestartLevel();
            visualTransform.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameOverEvent>(HandleGameOver);
        }
    }
}
using Project.Core.Events;
using Project.Core.Game;
using Project.Game;
using UnityEngine;

namespace Project.UI
{
    public class LevelFinishedMenu : MonoBehaviour
    {
        private IEventBus _eventBus;
        private IGameService _gameService;
        
        [SerializeField] private Transform visualTransform;
        [SerializeField] private CustomButton nextLevelButton;

        public void Init(IEventBus eventBus, IGameService gameService)
        {
            _eventBus = eventBus;
            _gameService = gameService;
            
            _eventBus.Subscribe<GameLevelFinishedEvent>(HandleGameLevelFinished);
            
            if (nextLevelButton == null)
                return;
                
            nextLevelButton.onPressed.AddListener(TriggerOpenNextLevel);
            visualTransform.gameObject.SetActive(false);
        }

        private void HandleGameLevelFinished(GameLevelFinishedEvent obj)
        {
            if (visualTransform == null)
                return;
            
            visualTransform.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(_gameService.HasNextLevel());
        }

        private void TriggerOpenNextLevel()
        {
            _gameService.ToNextLevel();
            visualTransform.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (nextLevelButton != null)
                nextLevelButton.onPressed.RemoveListener(TriggerOpenNextLevel);
        }
    }
}
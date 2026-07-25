using System;
using PrimeTween;
using Project.Core.Game;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class LevelFinishedMenu : GameUIMenu
    {
        private IGameService _gameService;
        public event Action OnHideRequested;
        
        [SerializeField] private CustomButton nextLevelButton;
        
        [Inject]
        public void Construct(IGameService gameService)
        {
            _gameService = gameService;
        }
        
        private void OnEnable()
        {
            if (nextLevelButton == null)
            {
                Debug.LogError($"[LevelFinishedMenu]: {nameof(nextLevelButton)} is null");
                return;
            }
            nextLevelButton.onPressed.AddListener(TriggerOpenNextLevel);
        }

        private void OnDisable()
        {
            if (nextLevelButton == null)
            {
                Debug.LogError($"[LevelFinishedMenu]: {nameof(nextLevelButton)} is null");
                return;
            }
            nextLevelButton.onPressed.RemoveListener(TriggerOpenNextLevel);
        }

        public override Tween PlayAppear()
        {
            nextLevelButton.gameObject.SetActive(_gameService.HasNextLevel());
            return base.PlayAppear();
        }

        private void TriggerOpenNextLevel()
        {
            _gameService.ToNextLevel();
            OnHideRequested?.Invoke();
        }
    }
}
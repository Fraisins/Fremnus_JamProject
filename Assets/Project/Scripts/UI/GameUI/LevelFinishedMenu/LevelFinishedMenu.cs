using System;
using PrimeTween;
using Project.Core.Game;
using UnityEngine;

namespace Project.UI
{
    public class LevelFinishedMenu : GameUIMenu
    {
        private IGameService _gameService;
        public event Action OnHideRequested;
        
        [SerializeField] private CustomButton nextLevelButton;

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

        public void Init(IGameService gameService)
        {
            _gameService = gameService;
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
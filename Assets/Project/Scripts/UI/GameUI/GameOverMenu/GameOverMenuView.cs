using System;
using Project.Core.Events;
using Project.Core.Game;
using UnityEngine;

namespace Project.UI
{
    public class GameOverMenuView : GameUIMenu
    {
        private IGameService _gameService;
        public event Action OnHideRequested;

        public void Init(IGameService gameService)
        {
            _gameService = gameService;
        }

        public void Retry()
        {
            _gameService.RestartLevel();
            OnHideRequested?.Invoke();
        }
    }
}
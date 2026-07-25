using System;
using Project.Core.Game;
using VContainer;

namespace Project.UI
{
    public class GameOverMenuView : GameUIMenu
    {
        private IGameService _gameService;
        public event Action OnHideRequested;

        [Inject]
        public void Construct(IGameService gameService)
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
using System;
using Project.Core.Events;
using UnityEngine;
using VContainer.Unity;

namespace Project.UI
{
    public class PauseMenuHandler : IInitializable, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly PauseMenuView _pauseMenu;
        
        public PauseMenuHandler(IEventBus eventBus, GameUIView gameUIView)
        {
            _eventBus = eventBus;

            _pauseMenu = gameUIView.PauseMenu;
            if (_pauseMenu == null)
                throw new NullReferenceException("[PauseMenuService] PauseMenu is null!");
        }

        public void Initialize()
        {
            _eventBus.Subscribe<GamePausedEvent>(ShowMenu);
            _eventBus.Subscribe<GameResumedEvent>(HideMenu);
            
            _pauseMenu.gameObject.SetActive(false);
        }

        private void ShowMenu(GamePausedEvent evt)
        {
            _pauseMenu.gameObject.SetActive(true);
        }

        private void HideMenu(GameResumedEvent obj)
        {
            _pauseMenu.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<GamePausedEvent>(ShowMenu);
            _eventBus.Unsubscribe<GameResumedEvent>(HideMenu);
        }
    }
}
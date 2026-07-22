using System;
using Project.Core;
using Project.Core.Events;
using VContainer.Unity;

namespace Project.UI
{
    public class LoadScreenService : IStartable, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly LoadScreenView _loadScreenView;


        public LoadScreenService(LoadScreenView loadScreenView, IEventBus eventBus)
        {
            _loadScreenView = loadScreenView;
            
            _eventBus = eventBus;
            _eventBus.Subscribe<GameStateChangedEvent>(HandleStateChanged);
        }

        public void Start()
        {
            _loadScreenView.gameObject.SetActive(false);
        }
        
        #region Show / Hide load screen
        
        private void ShowLoadScreen()
        {
            _loadScreenView.gameObject.SetActive(true);
            _loadScreenView.StartAnimation();
        }

        private void HideLoadScreen()
        {
            _loadScreenView.StopAnimation();
            _loadScreenView.gameObject.SetActive(false);
        }
        
        #endregion

        private void HandleStateChanged(GameStateChangedEvent evt)
        {
            if (evt.state == GameState.Loading)
                ShowLoadScreen();
            else
                HideLoadScreen();
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<GameStateChangedEvent>(HandleStateChanged);
        }
    }
}
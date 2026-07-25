using System;
using Project.Core;
using Project.Core.Events;
using VContainer.Unity;

namespace Project.Sound
{
    public class MusicHandler : IInitializable, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly ISoundService _soundService;
        private readonly MusicSettingsSO _settings;

        public MusicHandler(IEventBus eventBus, ISoundService soundService, 
            MusicSettingsSO settings)
        {
            _eventBus = eventBus;
            _soundService = soundService;
            _settings = settings;
        }

        public void Initialize()
        {
            _soundService.PlayMusic(_settings.GameMusic);
            //_eventBus.Subscribe<GameStateChangedEvent>(HandleGameStateChanged);
        }

        // private void HandleGameStateChanged(GameStateChangedEvent evt)
        // {
        //     if (evt.state == GameState.Game)
        //     {
        //         _soundService.PlayMusic(_settings.GameMusic);
        //         return;
        //     }
        //
        //     if (evt.state == GameState.Menu)
        //     {
        //         _soundService.StopMusic();
        //     }
        // }

        public void Dispose()
        {
            //_eventBus.Unsubscribe<GameStateChangedEvent>(HandleGameStateChanged);
        }
    }
}
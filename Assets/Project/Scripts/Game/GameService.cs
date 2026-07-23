using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Core.Events;
using Project.Core.Game;
using Project.Core.Numbers;
using Project.Core.Robot;
using UnityEngine;

namespace Project.Game
{
    public class GameService : IGameService, IDisposable
    {
        private readonly GameLevelChoiceHolder _levelChoiceHolder;
        private readonly GameSettingsSO _gameSettings;
        
        private readonly IEventBus _eventBus;
        private readonly IHpNumbersService _hpNumbersService;
        private readonly IModMachineService _modMachineService;
        private readonly IRobotService _robotService;
        private GameLevelSettingsSO _currentLevelSettings;
        private ISceneService _sceneService;

        private CancellationTokenSource _gameCts;
        
        private GameLevelSettingsSO CurrentLevelSettings => 
            GetLevelSettings(_levelChoiceHolder.currentLevelName);
        
        public GameService(
            IEventBus eventBus, GameLevelChoiceHolder levelChoiceHolder, IHpNumbersService hpNumbersService, IModMachineService modMachineService,
            IRobotService robotService, GameSettingsSO gameSettings, ISceneService sceneService)
        {
            _eventBus = eventBus;
            _levelChoiceHolder = levelChoiceHolder;
            _hpNumbersService = hpNumbersService;
            _modMachineService = modMachineService;
            _robotService = robotService;
            _gameSettings = gameSettings;
            _sceneService = sceneService;
        }

        public void InitScene()
        {;
            _currentLevelSettings = CurrentLevelSettings;
            SetLevelSettings(_currentLevelSettings);
            
            _eventBus.Publish(new GameInitEvent());
            
            _gameCts = new CancellationTokenSource();
            _hpNumbersService.CreateNumbers(4);
            _modMachineService.CreateModificators(4);
            _robotService.StartRobotLoop(_gameCts.Token).Forget();
        }

        public void FinishScene()
        {
            _robotService.StopRobotLoop();
            _hpNumbersService.Clear();
            _modMachineService.ForceReset();
            
            _gameCts?.Cancel();
            _gameCts?.Dispose();
            _gameCts = null;
        }

        #region Level

        private GameLevelSettingsSO GetLevelSettings(string levelName)
        {
            if (!_gameSettings.LevelSettingsDict.TryGetValue(levelName, out GameLevelSettingsSO levelSettings))
            {
                return null;
            }
            return levelSettings;
        }

        private void SetLevelSettings(GameLevelSettingsSO levelSettings)
        {
            _robotService.SetLevelSettings(levelSettings);
        }
        
        public void ToNextLevel()
        {
            _currentLevelSettings = CurrentLevelSettings;
            if (_currentLevelSettings == null || string.IsNullOrEmpty(_currentLevelSettings.NextLevelName))
                return;

            _levelChoiceHolder.currentLevelName = _currentLevelSettings.NextLevelName;
            RestartLevel();
        }

        public bool HasNextLevel()
        {
            _currentLevelSettings = GetLevelSettings(_levelChoiceHolder.currentLevelName);
            return !string.IsNullOrEmpty(_currentLevelSettings.NextLevelName);
        }

        #endregion


        public void RestartLevel()
        {
            FinishScene();
            InitScene();
        }

        public void Dispose()
        {
            _gameCts?.Dispose();
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Core.Events;
using Project.Core.Game;
using Project.Core.Numbers;
using Project.Core.Robot;
using Project.Core.SaveSystem;
using UnityEngine;

namespace Project.Game
{
    public class GameService : IGameService, IDisposable
    {
        private bool _isGameFinished = false;
        
        private readonly GameLevelChoiceHolder _levelChoiceHolder;
        private readonly GameSettingsSO _gameSettings;
        
        private readonly IEventBus _eventBus;
        private readonly IHpNumbersService _hpNumbersService;
        private readonly IModMachineService _modMachineService;
        private readonly IRobotService _robotService;
        private GameLevelSettingsSO _currentLevelSettings;
        private ISaveService _saveService;

        private CancellationTokenSource _gameCts;
        
        public GameService(
            IEventBus eventBus, GameLevelChoiceHolder levelChoiceHolder, IHpNumbersService hpNumbersService, IModMachineService modMachineService,
            IRobotService robotService, GameSettingsSO gameSettings, ISceneService sceneService, 
            ISaveService saveService)
        {
            _eventBus = eventBus;
            _levelChoiceHolder = levelChoiceHolder;
            _hpNumbersService = hpNumbersService;
            _modMachineService = modMachineService;
            _robotService = robotService;
            _gameSettings = gameSettings;
            _saveService = saveService;

            _robotService.OnRobotsOrderFinished += HandleRobotsOrderFinished;
            _eventBus.Subscribe<GameOverEvent>(HandleGameOver);
        }

        public void InitScene()
        {;
            _isGameFinished = false;
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
            
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        #region Level
        
        private GameLevelSettingsSO CurrentLevelSettings => 
            GetLevelSettings(_levelChoiceHolder.currentLevelName);
        
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

        #region Handlers

        private void HandleGameOver(GameOverEvent evt)
        {
            FinishScene();
            _isGameFinished = true;
        }

        private void HandleRobotsOrderFinished(RobotsFinishedResult result)
        {
            if (_isGameFinished) // CHECKS IF GAME OVER HAPPENED AT THE LAST LINE!
                return;

            if (result.successAmount == 0)
            {
                _eventBus.Publish<GameOverEvent>(default);
                return;
            }
            
            _isGameFinished = true;

            GameLevelFinishedEvent finEvt = new(
                _levelChoiceHolder.currentLevelName,
                result.successAmount,
                result.fullAmount);
            
            _saveService.SaveFinishedLevel(_levelChoiceHolder.currentLevelName, finEvt);
            _eventBus.Publish(finEvt);
        }

        #endregion

        public void Dispose()
        {
            _gameCts?.Cancel();
            _gameCts?.Dispose();
            _gameCts = null;
            
            _robotService.OnRobotsOrderFinished -= HandleRobotsOrderFinished;
            _eventBus.Unsubscribe<GameOverEvent>(HandleGameOver);
        }
    }
}
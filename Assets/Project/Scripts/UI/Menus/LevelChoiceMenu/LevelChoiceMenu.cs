using System.Collections.Generic;
using Project.Core;
using Project.Core.Game;
using Project.Core.SaveSystem;
using Project.Game;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class LevelChoiceMenu : Submenu
    {
        private GameLevelChoiceHolder _levelChoiceHolder;
        private ISceneService _sceneService;
        private ISaveService _saveService;
        private GameSettingsSO _gameSettings;
        
        [SerializeField] public List<LevelChoiceView> levelChoiceViews;

        [Inject]
        public void Construct(GameLevelChoiceHolder levelChoiceHolder,
            ISceneService sceneService, 
            ISaveService saveService,
            GameSettingsSO gameSettings)
        {
            _levelChoiceHolder = levelChoiceHolder;
            _sceneService = sceneService;
            _saveService = saveService;
            _gameSettings = gameSettings;
        }
        
        private void Awake()
        {
            foreach (LevelChoiceView levelChoiceView in levelChoiceViews)
            {
                levelChoiceView.OnLevelPicked += HandleLevelChoice;
            }
        }

        public override void OnOpened()
        {
            SetLevels();
        }

        private void HandleLevelChoice(string levelName)
        {
            _levelChoiceHolder.currentLevelName = levelName;
            _sceneService.OpenGameFromMenu();
        }

        private void SetLevels()
        {
            HashSet<string> levelNamesAvailable = new() { levelChoiceViews[0].LevelName };
            
            for (int i = 0; i < levelChoiceViews.Count; i++)
            {
                if (!_saveService.HasSavedLevel(levelChoiceViews[i].LevelName))
                    continue;
                
                if (_gameSettings.LevelSettingsDict.TryGetValue(
                        levelChoiceViews[i].LevelName, out GameLevelSettingsSO levelSettings))
                {
                    if (levelSettings != null && !string.IsNullOrEmpty(levelSettings.NextLevelName))
                        levelNamesAvailable.Add(levelSettings.NextLevelName);
                }
            }

            foreach (LevelChoiceView levelChoiceView in levelChoiceViews)
            {
                bool available = levelNamesAvailable.Contains(levelChoiceView.LevelName);
                levelChoiceView.SetAvailable(available);
                levelChoiceView.SetAdditionalInfoText(GetLevelAdditionalText(levelChoiceView));
            }
        }

        private string GetLevelAdditionalText(LevelChoiceView levelChoiceView)
        {
            string levelName = levelChoiceView.LevelName;
            
            if (!_gameSettings.LevelSettingsDict.TryGetValue(levelName, out GameLevelSettingsSO levelSettings))
                return string.Empty;

            int robotsAmount = levelSettings.robots.RobotAmount;
            bool hasSavedValue = false;
            int savedValue = 0;

            if (_saveService.TryGetLevelData(levelName, out SaveData.LevelData levelData))
            {
                hasSavedValue = true;
                savedValue = levelData.robotSuccessValue;
            }

            return hasSavedValue ? $"{savedValue} / {robotsAmount}" : "NO DATA";
        }

        private void OnDestroy()
        {
            foreach (LevelChoiceView levelChoiceView in levelChoiceViews)
            {
                levelChoiceView.OnLevelPicked -= HandleLevelChoice;
            }
        }
    }
}
using Project.Core.Events;
using UnityEngine;

namespace Project.Core.SaveSystem
{
    public class SaveService : ISaveService
    {
        private readonly SaveData _saveData;

        public SaveService(IEventBus eventBus)
        {
            _saveData = SaveDataConverter.Load();
        }
        
        public void SaveFinishedLevel(string levelName, GameLevelFinishedEvent evt)
        {
            SaveData.LevelData levelData = new(evt.successRobots);
            _saveData.SetLevelData(levelName, levelData);
            SaveDataConverter.Save(_saveData);
        }

        public bool HasSavedLevel(string levelName)
            => _saveData.LevelsInformation.ContainsKey(levelName);
        
        public bool TryGetLevelData(string levelName, out SaveData.LevelData levelData)
            => _saveData.LevelsInformation.TryGetValue(levelName, out levelData);
    }
}
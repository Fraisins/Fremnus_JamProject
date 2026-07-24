using System.Collections.Generic;

namespace Project.Core.SaveSystem
{
    public class SaveData
    {
        private readonly Dictionary<string, LevelData> _levelsInformation;
        public IReadOnlyDictionary<string, LevelData> LevelsInformation => _levelsInformation;
        
        public struct LevelData
        {
            public readonly int robotSuccessValue;

            public LevelData(int robotSuccessValue)
            {
                this.robotSuccessValue = robotSuccessValue;
            }
        }

        public SaveData(Dictionary<string, LevelData> levelsInformation)
        {
            if (levelsInformation == null)
                _levelsInformation = new Dictionary<string, LevelData>();
            else
                _levelsInformation = levelsInformation;
        }

        public void SetLevelData(string levelName, LevelData levelData)
        {
            _levelsInformation[levelName] = levelData;
        }
    }
}
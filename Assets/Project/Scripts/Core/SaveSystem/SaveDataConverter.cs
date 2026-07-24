using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.SaveSystem
{
    public static class SaveDataConverter
    {
        private const string SaveKey = "FemnusJam_SaveData";
        
        [Serializable]
        private class SaveDataJson
        {
            public List<LevelEntry> levels = new();
        }

        [Serializable]
        private struct LevelEntry
        {
            public string levelId;
            public int robotSuccessValue;
        }

        public static void Save(SaveData data)
        {
            SaveDataJson json = new SaveDataJson();

            foreach ((string levelName, SaveData.LevelData levelData) in data.LevelsInformation)
            {
                json.levels.Add(new LevelEntry
                {
                    levelId = levelName,
                    robotSuccessValue = levelData.robotSuccessValue
                });
            }
            
            string jsonString = JsonUtility.ToJson(json);
            PlayerPrefs.SetString(SaveKey, jsonString);
            PlayerPrefs.Save();
        }

        public static SaveData Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
                return new SaveData(new Dictionary<string, SaveData.LevelData>());
            
            string jsonString = PlayerPrefs.GetString(SaveKey);
            SaveDataJson json = JsonUtility.FromJson<SaveDataJson>(jsonString);
            
            Dictionary<string, SaveData.LevelData> levels = new();

            if (json?.levels != null)
            {
                foreach (LevelEntry entry in json.levels)
                {
                    levels[entry.levelId] = new SaveData.LevelData(
                        entry.robotSuccessValue
                        );
                }
            }
            
            return new SaveData(levels);
        }
        
        public static bool HasSave() 
            => PlayerPrefs.HasKey(SaveKey);
    }
}
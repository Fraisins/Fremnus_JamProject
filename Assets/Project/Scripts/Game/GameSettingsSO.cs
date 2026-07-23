using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Game
{
    [CreateAssetMenu (menuName = "FremnusJam/GameSettingsSO", fileName = "GameSettingsSO")]
    public class GameSettingsSO : ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        public class Robots
        {
            [field: SerializeField] public float requestTime = 10f;
        }

        [Serializable]
        public class HpNumbers
        {
            [field: SerializeField] public float damageTimer = 5f;
        }
        
        public Robots robots;
        public HpNumbers hpNumbers;

        [Serializable]
        public class LevelData
        {
            public string levelName;
            public GameLevelSettingsSO levelSettings;
        }
        
        [SerializeField] private List<LevelData> levels;
        
        private readonly Dictionary<string, GameLevelSettingsSO> _levelSettingsDict = new();
        public IReadOnlyDictionary<string, GameLevelSettingsSO> LevelSettingsDict => _levelSettingsDict;

        public void OnAfterDeserialize()
        {
            _levelSettingsDict.Clear();
            if (levels == null) return;

            foreach (LevelData level in levels)
            {
                if (!string.IsNullOrEmpty(level.levelName) && !_levelSettingsDict.ContainsKey(level.levelName))
                {
                    _levelSettingsDict.Add(level.levelName, level.levelSettings);
                }
            }
        }
        
        public void OnBeforeSerialize()
        {
            //
        }
    }
}
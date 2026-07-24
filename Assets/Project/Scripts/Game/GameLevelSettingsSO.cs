using Project.Game.Robot;
using UnityEngine;

namespace Project.Game
{
    [CreateAssetMenu (menuName = "FremnusJam/GameLevelSettingsSO", fileName = "GameLevelSettingsSO")]
    public class GameLevelSettingsSO : ScriptableObject
    {
        [field: SerializeField] public string NextLevelName { get; private set; }
        public RobotRequestSettings robots;
    }
}
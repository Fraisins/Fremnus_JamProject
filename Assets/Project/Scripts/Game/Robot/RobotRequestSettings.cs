using System;
using UnityEngine;

namespace Project.Game.Robot
{
    [Serializable]
    public class RobotRequestSettings
    {
        [field: SerializeField] public int MinAmount { get; private set; } = 2;
        [field: SerializeField] public int MaxAmount { get; private set; } = 3;

        [field: SerializeField] public int MinPositive { get; private set; } = 1;
        [field: SerializeField] public int MinNegative { get; private set; } = 1;
        
        [field: SerializeField] public int RobotAmount { get; private set; } = 10;
    }
}
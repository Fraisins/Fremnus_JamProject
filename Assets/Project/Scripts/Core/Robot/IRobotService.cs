using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Game;
using Project.Game.Robot;

namespace Project.Core.Robot
{
    public interface IRobotService
    {
        RobotRequest Request { get; }
        RobotRequestTimer Timer { get; }
        RobotOrder Order { get; }

        public event System.Action OnInitialized;
        
        void SetConveyor(RobotConveyor robotConveyor);
        void SetLevelSettings(GameLevelSettingsSO settings);
        
        UniTaskVoid StartRobotLoop(CancellationToken outsideToken = default);
        void StopRobotLoop();
        void ForceFinishCurrentTimer();
    }
}
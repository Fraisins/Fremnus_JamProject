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
        public event System.Action<RobotRequest> OnRequestStarted;
        public event System.Action<RobotRequestCheckResult> OnRequestFinished;
        public event System.Action<RobotsFinishedResult> OnRobotsOrderFinished;
        
        void SetConveyor(RobotConveyor robotConveyor);
        void SetLevelSettings(GameLevelSettingsSO settings);
        
        UniTaskVoid StartRobotLoop(CancellationToken outsideToken = default);
        void StopRobotLoop();
        void ForceFinishCurrentTimer();
    }
}
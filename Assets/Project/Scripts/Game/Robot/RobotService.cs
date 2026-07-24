using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core.Events;
using Project.Core.Numbers;
using Project.Core.Robot;
using UnityEngine;

namespace Project.Game.Robot
{
    public class RobotService : IRobotService, IDisposable
    {
        private readonly GameSettingsSO.Robots _settings;

        private readonly RobotOrder _order = new();
        private RobotConveyor _conveyor;
        private readonly IEventBus _eventBus;
        private readonly IHpNumbersService _hpNumbersService;

        private readonly RobotRequest _request = new();
        private RobotRequestGenerator _requestGenerator;
        private readonly RobotRequestTimer _timer = new();
        private CancellationTokenSource _forceTimerCts;
        
        private GameLevelSettingsSO _levelSettings;
        
        private readonly RobotRequestChecker _requestChecker;
        private readonly RobotResultApplierService _resultApplierService;

        private CancellationTokenSource _loopCts;
        
        public RobotRequest Request => _request;
        public RobotRequestTimer Timer => _timer;
        public RobotOrder Order => _order;
        
        private UniTask _loopTask;
        
        public event Action OnInitialized;
        public event Action<RobotRequest> OnRequestStarted;
        public event Action<RobotRequestCheckResult> OnRequestFinished;
        public event Action<RobotsFinishedResult> OnRobotsOrderFinished;

        public RobotService(GameSettingsSO settings,
            IHpNumbersService hpNumbersService)
        {
            _settings = settings.robots;
            _hpNumbersService = hpNumbersService;
            _requestChecker = new RobotRequestChecker(hpNumbersService);
            _resultApplierService = new RobotResultApplierService(hpNumbersService);
        }

        public void SetLevelSettings(GameLevelSettingsSO settings)
        {
            _levelSettings = settings;
            _requestGenerator = new RobotRequestGenerator(settings.robots, _hpNumbersService);
            OnInitialized?.Invoke();
        }
        
        public void SetConveyor(RobotConveyor robotConveyor)
        {
            _conveyor = robotConveyor ?? throw new ArgumentNullException(nameof(robotConveyor));
        }

        public async UniTaskVoid StartRobotLoop(CancellationToken outsideToken = default)
        {
            StopRobotLoop(); // cancel previous loop
            
            _order.Recreate(_levelSettings.robots.RobotAmount);
            
            _loopCts = CancellationTokenSource.CreateLinkedTokenSource(outsideToken);
            CancellationToken token = _loopCts.Token;

            try
            {
                while (_order.CurrentIndex < _order.RobotsStates.Count - 1)
                {
                    token.ThrowIfCancellationRequested();
                    await NextRobot(token);
                }
                OnRobotsOrderFinished?.Invoke(new RobotsFinishedResult(
                    _order.CountSuccessRobots(),
                    _order.RobotsStates.Count));
            }
            catch (OperationCanceledException)
            {
                // stopped
            }
        }

        private async UniTask NextRobot(CancellationToken token)
        {
            ForceFinishCurrentTimer();
            _forceTimerCts = new CancellationTokenSource();
                
            using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                token, _forceTimerCts.Token);
            CancellationToken combinedToken = linkedCts.Token;

            try
            {
                _order.NextRobot();
                _requestGenerator.FillRequest(_request);

                combinedToken.ThrowIfCancellationRequested();
                await _conveyor.AddRobotAndMoveToStop();
                combinedToken.ThrowIfCancellationRequested();
                
                OnRequestStarted?.Invoke(_request);
                await _timer.StartTimer(_settings.requestTime, _forceTimerCts.Token).SuppressCancellationThrow();
                
                if (token.IsCancellationRequested) // NOT TIMER!
                    return;

                RobotRequestCheckResult result = _requestChecker.CheckRequest(_request);
                if (result.FinishedPositive)
                {
                    _order.SuccessCurrentRobot();
                    
                    _resultApplierService.Apply(result);
                    OnRequestFinished?.Invoke(result);
                    
                    await _conveyor.MoveRobotToEnd();
                }
                else
                {
                    _order.FailCurrentRobot();
                    
                    _resultApplierService.Apply(result);
                    OnRequestFinished?.Invoke(result);
                    
                    await _conveyor.ReturnRobotToStart();
                }
            }
            catch (OperationCanceledException)
            {
                // cancelled
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public void StopRobotLoop()
        {
            ForceFinishCurrentTimer();

            if (_loopCts != null)
            {
                _loopCts.Cancel();
                _loopCts.Dispose();
                _loopCts = null;
            }
        }

        public void ForceFinishCurrentTimer()
        {
            _forceTimerCts?.Cancel();
            _forceTimerCts?.Dispose();
            _forceTimerCts = null;
        }

        public void Dispose()
        {
            _order.Clear();
            StopRobotLoop();
        }
    }
}
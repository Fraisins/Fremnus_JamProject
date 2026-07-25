using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core.Game;
using Project.Core.Pause;
using Project.Core.Robot;
using VContainer.Unity;

namespace Project.UI.Tutorial
{
    public class TutorialHandler : IInitializable, IDisposable
    {
        private bool _tutorialFinished = false;
        
        private readonly IRobotService _robotService;
        private readonly GameLevelChoiceHolder _levelHolder;
        private readonly TutorialLevelManager _tutorialLevelManager;
        private readonly IPauseService _pauseService;

        private CancellationTokenSource _cts;
        
        public TutorialHandler(IRobotService robotService, GameLevelChoiceHolder levelHolder,
            TutorialLevelManager tutorialLevelManager, IPauseService pauseService)
        {
            _robotService = robotService;
            _levelHolder = levelHolder;
            _tutorialLevelManager = tutorialLevelManager;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            _robotService.OnRequestStarted += HandleOnRequestStarted;
        }

        private void HandleOnRequestStarted(RobotRequest request)
        {
            _robotService.OnRequestStarted += HandleOnRequestStarted;
            
            if (_tutorialFinished)
                return;
            
            _cts = new CancellationTokenSource();
            HandleRequestStartedWithDelay(0.18f, _cts.Token).Forget();
        }

        private async UniTaskVoid HandleRequestStartedWithDelay(float delayTime, CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            
            if (_levelHolder.currentLevelName != "Level0") // hardcoded values... my favourite
                return;
            
            _pauseService.Pause(false);
            _pauseService.SetCanBeResumed(false);
            _tutorialLevelManager.OnTutorialFinished += HandleTutorialFinished;
            _tutorialLevelManager.StartTutorial();
        }

        private void HandleTutorialFinished()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            
            _tutorialFinished = true;
            _tutorialLevelManager.OnTutorialFinished -= HandleTutorialFinished;
            _pauseService.SetCanBeResumed(true);
            _pauseService.Resume();
        }
        
        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            
            _tutorialLevelManager.OnTutorialFinished -= HandleTutorialFinished;
            _robotService.OnRequestStarted -= HandleOnRequestStarted;
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core.Robot
{
    public class RobotRequestTimer
    {
        private CancellationTokenSource _cts;
        
        public bool IsRunning { get; private set; }
        public float ElapsedTime { get; private set; }
        public float Time { get; private set; }
        
        public event Action<bool> OnIsRunningChanged;

        public async UniTask StartTimer(float time, CancellationToken externalToken = default)
        {
            StopTimer();
            
            _cts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            CancellationToken token = _cts.Token;

            SetIsRunning(true);
            ElapsedTime = 0f;
            Time = time;
            
            try
            {
                while (ElapsedTime < Time)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                    ElapsedTime += UnityEngine.Time.deltaTime;
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            finally
            {
                SetIsRunning(false);
            }
        }

        private void SetIsRunning(bool isRunning)
        {
            if (isRunning == IsRunning)
                return;
            IsRunning = !IsRunning;
            OnIsRunningChanged?.Invoke(isRunning);
        }
        
        private void StopTimer()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            
            SetIsRunning(false);
        }
    }
}
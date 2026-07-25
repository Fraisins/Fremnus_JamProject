using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Project.Sound;
using UnityEngine;
using VContainer;

namespace Project.Game.Robot
{
    public class RobotConveyor : MonoBehaviour
    {
        [SerializeField] private Transform movingPoint;
        
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform stopPoint;
        [SerializeField] private Transform endPoint;
        
        [SerializeField] private float startToStopDuration = 0.6f;
        [SerializeField] private float stopToEndDuration = 1f;
        [SerializeField] private float delayBeforeMoveFromStop;
        
        [SerializeField] private Transform robotTrashContainer;

        [Header("Robot randomness")] 
        [SerializeField] private List<RobotComponent> robotVariations;
        [SerializeField] private List<Sprite> randomFaceTextures;

        [Header("Sound")] 
        [SerializeField] private AudioClip movingConveyorSound;
        
        private ISoundService _soundService;
        private RobotComponent _currentRobot;

        [Inject]
        private void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }
        
        private void Awake()
        {
            PrimeTweenConfig.warnStructBoxingAllocationInCoroutine = false; // shut
        }

        public async UniTask AddRobotAndMoveToStop()
        {
            if (this == null) return;
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            
            movingPoint.position = startPoint.position;
            AttachRandomRobotToMovingPoint();
            
            PlaySound(movingConveyorSound);
            
            await Tween.Position(
                movingPoint, 
                endValue: stopPoint.position, 
                duration: startToStopDuration, 
                ease: Ease.OutQuad
            ).ToUniTask(cancellationToken: token)
            .SuppressCancellationThrow();
        }

        public async UniTask ReturnRobotToStart()
        {
            if (this == null) return;
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            
            await UniTask.WaitForSeconds(delayBeforeMoveFromStop, cancellationToken: token);
            
            PlaySound(movingConveyorSound);
            
            bool isCanceled = await Tween.Position(
                movingPoint, 
                endValue: startPoint.position, 
                duration: startToStopDuration, 
                ease: Ease.OutQuad
            ).ToUniTask(cancellationToken: token)
            .SuppressCancellationThrow();
            
            if (isCanceled) return;
            
            ReturnRobot();
        }

        public async UniTask MoveRobotToEnd()
        {
            if (this == null) return;
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            
            await UniTask.WaitForSeconds(delayBeforeMoveFromStop, cancellationToken: token);
            
            PlaySound(movingConveyorSound);
            
            bool isCanceled = await Tween.Position(
                movingPoint, 
                endValue: endPoint.position, 
                duration: stopToEndDuration, 
                ease: Ease.OutQuad
            ).ToUniTask(cancellationToken: token)
            .SuppressCancellationThrow();
            
            if (isCanceled) return;
            
            ReturnRobot();
        }
        
        private void AttachRandomRobotToMovingPoint()
        {
            if (robotVariations == null || robotVariations.Count == 0) return;

            ReturnRobot();
            
            _currentRobot = TakeOutRandomRobot();;
            _currentRobot.transform.SetParent(movingPoint);
            _currentRobot.transform.localPosition = Vector3.zero;
        }

        private void ReturnRobot()
        {
            if (_currentRobot == null)
                return;
            
            _currentRobot.transform.SetParent(robotTrashContainer);
            _currentRobot.transform.localPosition = Vector3.zero;
            _currentRobot = null;
        }

        private RobotComponent TakeOutRandomRobot()
        { 
            if (robotVariations == null || robotVariations.Count == 0) return null;
            int randomIndex = UnityEngine.Random.Range(0, robotVariations.Count);
            RobotComponent robot = robotVariations[randomIndex];
            
            if (randomFaceTextures != null && randomFaceTextures.Count > 0)
                robot.SetFaceTexture(randomFaceTextures[UnityEngine.Random.Range(0, randomFaceTextures.Count)]);
            
            return robot;
        }

        #region Sound

        private void PlaySound(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("Sound clip is null");
                return;
            }
            if (_soundService == null)
            {
                Debug.LogWarning("[RobotConveyor]: SoundService is null");
                return;
            }
            _soundService.PlaySfx(clip);
        }

        #endregion
    }
}
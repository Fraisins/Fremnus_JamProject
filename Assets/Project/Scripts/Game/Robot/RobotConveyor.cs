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
        [Header("Conveyor Mesh & Shader")]
        [SerializeField] private Renderer conveyorRenderer;
        [SerializeField] private Vector2 speedMultiplier = new(0f, 1f);
        
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
        [SerializeField] private float minRotation = 12f;
        [SerializeField] private float maxRotation = 30f;

        [Header("Sound")] 
        [SerializeField] private AudioClip movingConveyorSound;
        
        private Material _conveyorMaterial; // cache
        
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
            
            if (conveyorRenderer != null)
                _conveyorMaterial = conveyorRenderer.material; // cache or else it will make multiple instances
            
            // Test().Forget();
        }

        // private async UniTaskVoid Test()
        // {
        //     await AddRobotAndMoveToStop();
        //     await UniTask.WaitForSeconds(0.5f);
        //     await MoveRobotToEnd();
        //
        //     await UniTask.WaitForSeconds(0.5f);
        //     
        //     await AddRobotAndMoveToStop();
        //     await UniTask.WaitForSeconds(0.5f);
        //     await ReturnRobotToStart();
        // }
        
        #region ConveyorMovement

        public async UniTask AddRobotAndMoveToStop()
        {
            if (this == null) return;
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            
            movingPoint.position = startPoint.position;
            AttachRandomRobotToMovingPoint();
            
            PlaySound(movingConveyorSound);

            await Sequence.Create()
                .Group(Tween.Position(movingPoint, 
                    endValue: stopPoint.position, 
                    duration: startToStopDuration, 
                    ease: Ease.OutQuad))
                .Group(AnimateConveyorOffset(startToStopDuration, 
                    Ease.OutQuad))
                .ToUniTask(cancellationToken: token)
                .SuppressCancellationThrow();
        }

        public async UniTask ReturnRobotToStart()
        {
            if (this == null) return;
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            
            await UniTask.WaitForSeconds(delayBeforeMoveFromStop, cancellationToken: token);
            
            PlaySound(movingConveyorSound);
            
            bool isCanceled = await Sequence.Create()
                .Group(Tween.Position(movingPoint, 
                    endValue: startPoint.position, 
                    duration: startToStopDuration, 
                    ease: Ease.OutQuad))
                .Group(AnimateConveyorOffset(
                    startToStopDuration, 
                    Ease.OutQuad,
                    directionMultiplier: -1f))
                .ToUniTask(cancellationToken: token)
                .SuppressCancellationThrow();
            
            if (isCanceled) 
                return;
            
            ReturnRobot();
        }

        public async UniTask MoveRobotToEnd()
        {
            if (this == null) return;
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            
            await UniTask.WaitForSeconds(delayBeforeMoveFromStop, cancellationToken: token);
            
            PlaySound(movingConveyorSound);
            
            bool isCanceled = await Sequence.Create()
                .Group(Tween.Position(movingPoint, 
                    endValue: endPoint.position, 
                    duration: stopToEndDuration, 
                    ease: Ease.OutQuad))
                .Group(AnimateConveyorOffset(
                    startToStopDuration, 
                    Ease.OutQuad))
                .ToUniTask(cancellationToken: token)
                .SuppressCancellationThrow();
            
            if (isCanceled) 
                return;
            
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

        private Tween AnimateConveyorOffset(float duration, Ease ease, float directionMultiplier = 1f)
        {
            if (_conveyorMaterial == null) return default;
            
            Vector2 startOffset = _conveyorMaterial.mainTextureOffset;
            Vector2 targetOffset = startOffset + speedMultiplier * directionMultiplier;

            return Tween.Custom(
                startValue: startOffset,
                endValue: targetOffset,
                duration: duration,
                ease: ease,
                onValueChange: newOffset => 
                    _conveyorMaterial.mainTextureOffset = newOffset);
        }

        #endregion

        #region Randomness
        
        private RobotComponent TakeOutRandomRobot()
        { 
            if (robotVariations == null || robotVariations.Count == 0) return null;
            int randomIndex = Random.Range(0, robotVariations.Count);
            RobotComponent robot = robotVariations[randomIndex];
            
            if (randomFaceTextures != null && randomFaceTextures.Count > 0)
                robot.SetFaceTexture(randomFaceTextures[Random.Range(0, randomFaceTextures.Count)]);
            
            float randomRot = Random.Range(minRotation, maxRotation);
            robot.transform.rotation = Quaternion.Euler(0f, randomRot, 0f);
            
            return robot;
        }

        #endregion

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
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

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
        [SerializeField] private List<GameObject> robotVariations;
        
        private GameObject _currentRobot;

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

            if (_currentRobot != null)
            {
                _currentRobot.transform.SetParent(robotTrashContainer);
                _currentRobot.transform.localPosition = Vector3.zero;
            }
            
            GameObject robot = robotVariations[UnityEngine.Random.Range(0, robotVariations.Count)];
            
            _currentRobot = robot;
            _currentRobot.transform.SetParent(movingPoint);
            _currentRobot.transform.localPosition = Vector3.zero;
        }

        private void ReturnRobot()
        {
            _currentRobot.transform.SetParent(robotTrashContainer);
            _currentRobot.transform.localPosition = Vector3.zero;
            _currentRobot = null;
        }
    }
}
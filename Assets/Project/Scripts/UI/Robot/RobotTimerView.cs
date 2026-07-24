using System;
using PrimeTween;
using Project.Core.Robot;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Scripts.UI
{
    public class RobotTimerView : MonoBehaviour
    {
        private IRobotService _robotService;
        private RobotRequestTimer _timer;
        
        [SerializeField] private Image fillImage;
        [SerializeField] private Transform visualTransform;
        
        [Header("Timer color")]
        [SerializeField] private Color baseTimerColor;
        [SerializeField] private Color lowTmeTimerColor;
        [SerializeField] private float lowTimePercentage = 0.25f;
        [SerializeField] private float colorChangeDuration = 0.15f;
        
        private bool _isActive = false;
        private bool _isLowTimeColor = false;
        private Tween _timerColorChange;

        [Inject]
        public void Construct(IRobotService robotService)
        {
            _robotService = robotService;
            _timer = robotService.Timer;
        }

        public void Update()
        {
            if (_timer == null)
                return;
            if (!_timer.IsRunning && visualTransform != null && _isActive)
            {
                _isActive = false;
            }
            else if (_timer.IsRunning && !_isActive)
            {
                _isActive = true;
                fillImage.color = baseTimerColor;
                _isLowTimeColor = false;
            }

            float fillAmount = 1f - _timer.ElapsedTime / _timer.Time;
            fillImage.fillAmount = fillAmount;
            
            SmoothColorChange(fillAmount);
        }

        private void SmoothColorChange(float remainingPercentage)
        {
            bool isLow = remainingPercentage <= lowTimePercentage;
            if (isLow == _isLowTimeColor)
                return;

            _isLowTimeColor = isLow;
            Color targetColor = isLow ? lowTmeTimerColor : baseTimerColor;
            
            if (_timerColorChange.isAlive)
                _timerColorChange.Stop();
            
            _timerColorChange = Tween.Color(
                fillImage,
                endValue: targetColor,
                duration: colorChangeDuration);
        }
        
        public void ForceFinishTimer() => 
            _robotService.ForceFinishCurrentTimer();

        private void OnDestroy()
        {
            if (_timerColorChange.isAlive)
                _timerColorChange.Stop();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (fillImage != null)
            {
                fillImage.color = baseTimerColor;
            }
        }
#endif
    }
}
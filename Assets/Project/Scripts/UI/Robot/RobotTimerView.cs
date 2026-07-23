using Project.Core.Robot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Scripts.UI
{
    public class RobotTimerView : MonoBehaviour
    {
        private IRobotService _robotService;
        private RobotRequestTimer _timer;
        private bool _isActive = false;

        [SerializeField] private Image fillImage;
        [SerializeField] private Transform visualTransform;

        [Inject]
        public void Construct(IRobotService robotService)
        {
            _robotService = robotService;
            _timer = robotService.Timer;
            
            if (!_timer.IsRunning)
                visualTransform.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (_timer == null)
                return;
            if (!_timer.IsRunning && visualTransform != null && _isActive)
            {
                _isActive = false;
                visualTransform.gameObject.SetActive(false);
            }
            else if (_timer.IsRunning && !_isActive)
            {
                _isActive = true;
                visualTransform.gameObject.SetActive(true);
            }

            fillImage.fillAmount = _timer.ElapsedTime / _timer.Time;
        }

        public void ForceFinishTimer() => 
            _robotService.ForceFinishCurrentTimer();
    }
}
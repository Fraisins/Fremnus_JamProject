using System;
using Project.Core.Robot;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class RobotLightView : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        private RobotState _currentState = RobotState.NotStarted;
        public event Action<RobotLightView, RobotState> OnStateChanged;

        public void SetState(RobotState state)
        {
            if (_currentState == state)
                return;
            
            _currentState = state;
            OnStateChanged?.Invoke(this, _currentState);
        }

        public void SetColor(Color color)
        {
            if (image == null)
                return;
            
            image.color = color;
        }
    }
}
using Project.Core.Robot;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class RobotLightView : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        [SerializeField] private Color activeColor;
        [SerializeField] private Color unactiveColor;
        [SerializeField] private Color failedColor;
        [SerializeField] private Color successColor;

        public void SetState(RobotState state)
        {
            Color color = state switch
            {
                RobotState.Active => activeColor,
                RobotState.Failed => failedColor,
                RobotState.Success => successColor,
                RobotState.NotStarted => unactiveColor,
                _ => Color.white
            };
            image.color = color;
        }
    }
}
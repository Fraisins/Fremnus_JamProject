using Project.Core.Robot;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class RobotRequestLetter : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;

        public void SetLabel(int value)
        {
            label.text = value.ToString();
        }
    }
}